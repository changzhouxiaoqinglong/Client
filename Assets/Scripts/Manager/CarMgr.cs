using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 车辆管理
/// </summary>

public class CarMgr
{
    /// <summary>
    /// 当前的车
    /// </summary>
    public List<CarBase> cars = new List<CarBase>();

    /// <summary>
    /// 车辆节点
    /// </summary>
    private Transform vehicleRoot;
    private Transform VehicleRoot
    {
        get
        {
            if (vehicleRoot == null)
            {
                GameObject vehicleGo = GameObject.Find("Vehicles");
                if (vehicleGo)
                {
                    vehicleRoot = vehicleGo.transform;
                }
                else
                {
                    vehicleRoot = new GameObject("Vehicles").transform;
                }
            }
            return vehicleRoot;
        }
    }

    public Train3DSceneCtrBase CurScene
    {
        get; set;
    }

    public CarMgr()
    {
        EventDispatcher.GetInstance().AddEventListener(EventNameList.UPDATE_CAR_PLAYER, UpdateTrainCarDatas);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.FLAG_TO_DRIVER, OnGetFlatMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.PROTECT, OnGetProtectNetMsg);
    }


    /// <summary>
    /// 更新训练车辆人物
    /// </summary>
    /// <param name="param"></param>
    private void UpdateTrainCarDatas(IEventParam param)
    {
        UpdateCars();
    }

    /// <summary>
    /// 更新车辆
    /// </summary>
    public void UpdateCars()
    {
        //参加训练的车人数据
        List<TrainMachineVarData> trainMachines = NetVarDataMgr.GetInstance()._NetVarData.TrainMachineDatas;
        foreach (var trainMachine in trainMachines)
        {
            CarBase car = cars.Find(_car => _car.MachineId == trainMachine.MachineId);
            //还没生成车辆
            if (car == null)
            {
                //生成车辆
                Vector3 vector3 = CurScene.terrainChangeMgr.GetTerrainPosByGis(trainMachine.InitPos.ToVector2());
                vector3 = new Vector3(vector3.x, vector3.y + 1, vector3.z);
                float rotateY;
                if (!float.TryParse(trainMachine.Rotate, out rotateY))
                {
                    rotateY = 0f;
                }
                //本地测试用
                //car = CreateCar(trainMachine.CarId, trainMachine.MachineId, new Vector3(1395f,95f,1393f),rotateY);
                car = CreateCar(trainMachine.CarId, trainMachine.MachineId, vector3, rotateY);
                if (trainMachine.IsSelf())
                {
                    //自己的车 启用相机
                    car.cameraChanger.SetEnable();
                    //当前是驾驶员位置 设置控制权
                    if (AppConfig.SEAT_ID == SeatType.DRIVE)
                    {
                        //这里延迟到下一帧处理，不然会有挡位为-1的情况
                        car.DelayInvoke(0, () =>
                        {
                            //设置控制车辆
                            VehicleInputMgr.GetInstance().VehicleController = car.vehicleCtr;
                            //启用车辆控制
                            VehicleInputMgr.GetInstance().SetEnable();
                        });
                    }
                }
                cars.Add(car);
            }
            //更新车上参加训练的人
            car.playerMgr.UpdatePlayerDatas(trainMachine.TrainUserDatas);
        }
    }

    /// <summary>
    /// 生成车辆
    /// </summary>
    /// <param name="carId">车id</param>
    public CarBase CreateCar(int carId, int machineId, Vector3 pos, float rotate)
    {
        Debug.Log("生成车辆 carId" + carId);
        ExCarData car = CarExDataMgr.GetInstance().GetDataById(carId);
        GameObject carObj = Resources.Load<GameObject>(car.Res);
        GameObject carIns = Object.Instantiate(carObj, VehicleRoot);
        CarBase carBase = carIns.GetComponent<CarBase>();
        carBase.SetPosition(pos);
        Vector3 beforeRotate = carBase.transform.localEulerAngles;
        carBase.transform.localEulerAngles = new Vector3(beforeRotate.x, rotate - 90f, beforeRotate.z);
        carBase.MachineId = machineId;
        cars.Add(carBase);
        return carBase;
    }

    public CarBase GetCarByMachineId(int machineId)
    {
        foreach (var item in cars)
        {
            if (item.MachineId == machineId)
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// 收到插旗消息
    /// </summary>
    private void OnGetFlatMsg(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            //是自己车里的人传来的
            if (tcpReceiveEvParam.netData.MachineId == AppConfig.MACHINE_ID)
            {
                //注意这里不会有位置信息，位置要用射线来算
                FlagToDriveModel model = JsonTool.ToObject<FlagToDriveModel>(tcpReceiveEvParam.netData.Msg);
                //当前在控制人物  就通过人物插旗
                if (InputCtrMgr.GetInstance().curInputCtr is PlayerCtr playerCtr)
                {
                    playerCtr.DoFlag(model.FlagType, model.Info);
                }
                else
                {
                    //在车上插旗
                    GetCarByMachineId(AppConfig.MACHINE_ID).DoFlag(model.FlagType, model.Info);
                }
            }
        }
    }

    /// <summary>
    /// 收到防护消息
    /// </summary>
    private void OnGetProtectNetMsg(IEventParam param)
    {

        #region 新版修改

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            Debug.Log("carmgr收到防护消息:" + tcpReceiveEvParam.netData.SeatId);
            ProtectModel model = JsonTool.ToObject<ProtectModel>(tcpReceiveEvParam.netData.Msg);

            //对应防护的人物
            List<PlayerCtr> players = GetPlayersCtr(tcpReceiveEvParam.netData.MachineId);
            if (model.IsProtect)
            {
                foreach(var player in players)
				{
                    if (player)
                    {
                        player.DoProtect();
                    }
                }

               
            }
            else
            {
                foreach (var player in players)
                {
                    if (player)
                    {
                        player.UnDoProtect();
                    }
                }  
            }


            //List<TrainMachineVarData> trainMachines = NetVarDataMgr.GetInstance()._NetVarData.TrainMachineDatas;
            //foreach (var trainMachine in trainMachines)
            //{
            //    if (trainMachine.IsSelf())
            //    {
            //        foreach (var seatData in trainMachine.TrainSeatDatas)
            //        {
            //            //对应防护的人物
            //            PlayerCtr player = GetPlayerCtr(tcpReceiveEvParam.netData.MachineId, seatData.SeatId);
            //            if (model.IsProtect)
            //            {
            //                if (player)
            //                {
            //                    player.DoProtect();
            //                }
            //            }
            //            else
            //            {
            //                if (player)
            //                {
            //                    player.UnDoProtect();
            //                }
            //            }
            //        }
            //    }

            //}


           
           
        }

		#endregion
		//////////old

		//if (param is TcpReceiveEvParam tcpReceiveEvParam)
		//      {
		//          Debug.Log("carmgr收到防护消息:" + tcpReceiveEvParam.netData.SeatId);
		//          ProtectModel model = JsonTool.ToObject<ProtectModel>(tcpReceiveEvParam.netData.Msg);
		//          //对应防护的人物
		//          PlayerCtr player = GetPlayerCtr(tcpReceiveEvParam.netData.MachineId, tcpReceiveEvParam.netData.SeatId);
		//          if (model.IsProtect)
		//          {
		//              if (player)
		//              {
		//                  player.DoProtect();
		//              }
		//          }
		//          else
		//          {
		//              if (player)
		//              {
		//                  player.UnDoProtect();
		//              }
		//          }
		//      }
	}




	/// <summary>
	/// 获得人物对象
	/// </summary>
	private PlayerCtr GetPlayerCtr(int machinId, int seatId)
    {
        CarBase car = GetCarByMachineId(machinId);
        if (car)
        {
            PlayerCtr player = car.playerMgr.GetPlayerCtrBySeatId(seatId);
            return player;
        }
        return null;
    }

    /// <summary>
    /// 获得一辆车上的所有的人物对象  新版修改
    /// </summary>
    private List<PlayerCtr> GetPlayersCtr(int machinId)
    {
        List<PlayerCtr> players = new List<PlayerCtr>();
       
        CarBase car = GetCarByMachineId(machinId);
        if (car)
        {         
            return car.playerMgr.GetPlayerCtr();
        }
        return null;
    }

    public void OnDestroy()
    {
        EventDispatcher.GetInstance().RemoveEventListener(EventNameList.UPDATE_CAR_PLAYER, UpdateTrainCarDatas);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.FLAG_TO_DRIVER, OnGetFlatMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.PROTECT, OnGetProtectNetMsg);

    }
}
