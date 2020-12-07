using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using wvr;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private StepsManager tutorialStepManager;
    [SerializeField] private TextMeshProUGUI intro;
    [SerializeField] private Color textColor;
    [SerializeField] private Image introHolder;
    [SerializeField] private float animationSpeed;
    [SerializeField] private GameObject fakeController, scenerioCanvas, appManager;
    [SerializeField] private HandManager hand;
    public bool isTutorialActivated = true;

    private Sequence sequence;
    private List<Image> _imgs = new List<Image>();
    private List<TextMeshProUGUI> _infoHolders = new List<TextMeshProUGUI>();
    private Image tempImg;
    private TextMeshProUGUI tempText;
    private bool onStart = false;


    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(animationSpeed);
        GreetingMessageAppear();

        yield return new WaitForSeconds(animationSpeed);
        MakeControllerAppear();

        yield return new WaitForSeconds(animationSpeed);
        sequence.Kill();
        sequence = null;
    }

    void Update()
    {
        if (sequence == null && onStart == false)
        {
            //Debug.LogWarning("Enabled");

            if(!isTutorialActivated || !GameManager.Instance.EnableTutorial)
                DisableTutorial();

            var startTutorial = WaveVR_Controller.Input(WVR_DeviceType.WVR_DeviceType_Controller_Right).GetPressUp(wvr.WVR_InputId.WVR_InputId_Alias1_Touchpad);
            var skipTutorial = WaveVR_Controller.Input(WVR_DeviceType.WVR_DeviceType_Controller_Right).GetPressUp(wvr.WVR_InputId.WVR_InputId_Alias1_Menu);

            if (startTutorial == true)
            {
                tutorialStepManager.gameObject.SetActive(true);
                _imgs.ForEach(i => i.gameObject.SetActive(false));
                _infoHolders.ForEach(info => info.gameObject.SetActive(false));
                transform.GetChild(0).gameObject.SetActive(false);
                hand.ControllerAppearance(false);
                hand.ControllerAppearance(true);
                onStart = true;

            }
            else if (skipTutorial == true)
            {
                hand.ControllerAppearance(false);
                hand.ControllerAppearance(true);
                DisableTutorial();
            }
        }
        else
            Debug.LogWarning("Waiting For Application to Load Tutorial !");
    }

    private void GreetingMessageAppear()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        sequence = DOTween.Sequence();
        sequence.Append(introHolder.DOColor(Color.white, animationSpeed));
        sequence.Append(intro.DOColor(textColor, animationSpeed));
    }

    private void MakeControllerAppear()
    {
        fakeController.SetActive(true);

        _imgs = fakeController.transform.GetComponentsInChildren<Image>().ToList();
        _imgs.ForEach(i => sequence.Append(i.DOColor(Color.white, animationSpeed)));

        _infoHolders = fakeController.transform.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        _infoHolders.ForEach(t => sequence.Append(t.DOColor(textColor, animationSpeed)));
    }

    #region Event Calls

    public void DisableTutorial()
    {
        scenerioCanvas.SetActive(true);
        //appManager.SetActive(true);
        fakeController.SetActive(false);
        tutorialStepManager.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void SetImage(Image img)
    {
        tempImg = img;
    }

    public void ImageAppear()
    {
        if (tempImg != null)
            tempImg.DOColor(Color.white, animationSpeed);
    }
    public void SetText(TextMeshProUGUI tmp)
    {
        tempText = tmp;
    }

    public void TextAppear()
    {
        if (tempText != null)
            tempText.DOColor(textColor, animationSpeed);
    }

    public void RotateController()
    {
        fakeController.transform.DORotate(new Vector3(5, -9, 0), animationSpeed);

        _imgs = fakeController.transform.GetComponentsInChildren<Image>().ToList();
        _imgs.ForEach(i => sequence.Append(i.DOColor(Color.white, animationSpeed)));

        _infoHolders = fakeController.transform.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        _infoHolders.ForEach(t => sequence.Append(t.DOColor(textColor, animationSpeed)));
    }
    #endregion
}
