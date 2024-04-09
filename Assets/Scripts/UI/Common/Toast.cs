
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Toast : MonoBehaviour
{
    private Text toastTip;

    private Transform toastContent;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        toastContent = transform.Find("bg");
        canvasGroup = toastContent.GetComponent<CanvasGroup>();
        toastTip = transform.Find("bg/Tip").GetComponent<Text>();
    }

    public void ShowTip(string tip)
    {
        toastTip.text = tip;
        toastContent.DOLocalMoveY(350, 0.5f).
            AppendInternal(2f).
            Append(canvasGroup.DOFade(0,1.5f))
            .AppendCallback(() =>
            {
                Destroy(gameObject);
            });
    }
}
