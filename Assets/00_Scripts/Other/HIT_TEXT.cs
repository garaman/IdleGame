using TMPro;
using UnityEngine;

public class HIT_TEXT : MonoBehaviour
{
    Vector3 target;
    Camera cam;
    public TextMeshProUGUI m_text;

    [SerializeField] GameObject m_Critical;

    float upRange = 0.0f;

    private void Start()
    {
        cam = Camera.main;
        
    }

    public void OnSave()
    {
        ReturnText();
    }

    private void OnDisable()
    {
        UI_SavingMode.m_OnSving -= OnSave;
    }
    public void Init(Vector3 pos, double damage,bool isCritical = false, bool isMonster = false , bool isHeal = false)
    {
        UI_SavingMode.m_OnSving += OnSave;

        pos.x += Random.Range(-0.3f, 0.3f);
        pos.z += Random.Range(-0.3f, 0.3f);

        target = pos;
        m_text.text = StringMethod.ToCurrencyString(damage);

        if(isMonster) { m_text.color = Color.red; }
        else {  m_text.color = Color.white;}

        transform.SetParent(BaseCanvas.instance.HOLDER_LAYER(1));

        m_Critical.SetActive(isCritical);
        if (isCritical)
        {
            m_text.color = Color.yellow;
        }
        else if (isHeal)
        {
            m_text.color = Color.green;
        }

        BaseManager.instance.Return_Pool(3.0f, this.gameObject, "HIT_TEXT");
    }

    private void Update()
    {
        if(BaseCanvas.isSave) { return; }

        Vector3 targetPos = new Vector3(target.x, target.y + upRange, target.z);
        transform.position = cam.WorldToScreenPoint(targetPos);
        if(upRange <= 0.5f)
        {
            upRange += Time.deltaTime;
        }
    }


    private void ReturnText()
    {
        BaseManager.Pool.m_pool_Dictionary["HIT_TEXT"].Return(this.gameObject);
    }

}
