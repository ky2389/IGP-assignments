using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SimpleDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{   [SerializeField]
    UnityEngine.UI.Image cardArt;
    [SerializeField]
    TextMeshProUGUI text_CardName;
    [SerializeField]
    TextMeshProUGUI text_CardCost;
    [SerializeField]
    Vector3 hoveringScale=  new Vector3(2f,2f,2f); //碰到卡就变大
    float hoveringY=200f;//碰到卡就向上移动
    Vector3 selectedScale = new Vector3(1.5f, 1.5f, 1.5f);//拖动的时候稍微变大
    float lerpSpeed = 3; //卡移动的速度
    [HideInInspector]
    public Vector2 targetPos=Vector2.zero;//这个是public因为card manager要用做anchor
    public Quaternion targetRot=Quaternion.identity;//和旋转角

    [HideInInspector]
    public RectTransform rectTrans;
    bool isDragging=false; 
    bool rollover=false;
    private CardManager cardManager;
    public CardProperty cardProperty;
    public void Init(CardProperty cardProperty, CardManager cM){
        this.cardProperty=cardProperty;
        cardManager=cM;
        cardArt.sprite=cardProperty.art;
        text_CardName.text=cardProperty.name;
        text_CardCost.text=cardProperty.cost.ToString();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTrans=GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        if((!rollover && !isDragging)||CardManager.gameStatus!=GameStatus.Ready){
            rectTrans.anchoredPosition=Vector2.Lerp(rectTrans.anchoredPosition,targetPos,Time.deltaTime*lerpSpeed);
            rectTrans.rotation=Quaternion.Lerp(rectTrans.rotation,rectTrans.parent.rotation*targetRot,Time.deltaTime*lerpSpeed);
        }
        else if(rollover && !isDragging){
            rectTrans.localScale=hoveringScale;
            rectTrans.anchoredPosition= new Vector2(targetPos.x,hoveringY);
            //and keep it straight
            rectTrans.rotation=Quaternion.Euler(Vector3.zero);
        }
    }
    public void OnPointerEnter(PointerEventData eventData){
        if(cardManager.target==null){ //必须没有正在drag的卡
            rollover=true;
            cardManager.RelocateAllCards(this);
        }
    }
    public void OnPointerExit(PointerEventData eventData){
        rollover=false;
        rectTrans.localScale = Vector3.one;
        cardManager.RelocateAllCards();

    }
    public void OnBeginDrag(PointerEventData eventData){
        if(eventData.button==PointerEventData.InputButton.Left){
            if(!isDragging){
                isDragging=true;
                cardManager.target=this;
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData){
        if(eventData.button==PointerEventData.InputButton.Left){
            if(isDragging){
                isDragging=false;
                //check if player is dragging the card to the drop zone
                if(CardManager.gameStatus==GameStatus.Ready){
                    cardManager.TakingEffectCheck();
                }
                cardManager.target=null;
                rectTrans.localScale=Vector3.one;
            }
        }
    }
    public void OnDrag(PointerEventData eventData){
        if(eventData.button==PointerEventData.InputButton.Left){
            if(isDragging){
                // Vector3 globalMousePos;
                // if(RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTrans,eventData.position,eventData.pressEventCamera,out globalMousePos)){
                //     rectTrans.position=globalMousePos;
                // }
                rectTrans.anchoredPosition+=eventData.delta;
                rectTrans.localScale=selectedScale;
            }
        }
    }

}
