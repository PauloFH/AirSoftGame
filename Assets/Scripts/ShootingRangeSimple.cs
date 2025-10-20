using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShootingRangeSimple : MonoBehaviour
{
    [Header("UI")]
    public Text txtPontuacao;
    
    [Header("Alvos")]
    public List<GameObject> alvosOriginais = new List<GameObject>();
    
    private int pontuacao = 0;
    private List<GameObject> prefabsAlvos = new List<GameObject>();
    private List<GameObject> alvosAtivos = new List<GameObject>();
    private List<Vector3> posicoesOriginais = new List<Vector3>();
    private List<Quaternion> rotacoesOriginais = new List<Quaternion>();
    
    void Start()
    {
        foreach (GameObject alvo in alvosOriginais)
        {
            if (alvo != null)
            {
                posicoesOriginais.Add(alvo.transform.position);
                rotacoesOriginais.Add(alvo.transform.rotation);
                GameObject template = Instantiate(alvo);
                template.SetActive(false);
                template.name = alvo.name + "_Template";
                prefabsAlvos.Add(template);
                alvosAtivos.Add(alvo);
                AlvoSimple script = alvo.GetComponent<AlvoSimple>();
                if (script != null)
                {
                    script.manager = this;
                }
            }
        }
        
        AtualizarPontuacao();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            ResetarJogo();
        }
    }
    
    public void AdicionarPonto()
    {
        pontuacao++;
        AtualizarPontuacao();
    }
    
    void AtualizarPontuacao()
    {
        if (txtPontuacao != null) {
            txtPontuacao.text = "Pontos: " + pontuacao;
        }
        else
        {
            Debug.LogError("txtPontuacao é NULL! Verifique se está atribuído no Inspector!");
        }
    }
    
    void ResetarJogo()
    {
        Debug.Log("=== RESETANDO JOGO ===");
        
        pontuacao = 0;
        AtualizarPontuacao();
        foreach (GameObject alvo in alvosAtivos)
        {
            if (alvo != null)
            {
                Destroy(alvo);
            }
        }
        alvosAtivos.Clear();
        for (int i = 0; i < prefabsAlvos.Count; i++)
        {
            if (prefabsAlvos[i] != null)
            {
                GameObject novoAlvo = Instantiate(prefabsAlvos[i], posicoesOriginais[i], rotacoesOriginais[i]);
                novoAlvo.SetActive(true);
                novoAlvo.name = prefabsAlvos[i].name.Replace("_Template", "");
                AlvoSimple script = novoAlvo.GetComponent<AlvoSimple>();
                if (script != null)
                {
                    script.manager = this;
                }
                
                alvosAtivos.Add(novoAlvo);
            }
        }
    }
    
    void OnDestroy()
    {
        foreach (GameObject template in prefabsAlvos)
        {
            if (template != null)
            {
                Destroy(template);
            }
        }
    }
}
