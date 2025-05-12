using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryIU : MonoBehaviour
{
    public CanvasGroup fadeGroup;           // 어두운 배경 (페이드용)
    public GameObject inventoryPanel;       // 실제 인벤토리 내용이 담긴 패널

    private bool isInventoryOpen = false;
    private float fadeDuration = 0.3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isInventoryOpen)
                StartCoroutine(OpenInventory());
            else
                StartCoroutine(CloseInventory());
        }
    }

    IEnumerator OpenInventory()
    {
        isInventoryOpen = true;
        inventoryPanel.SetActive(true);

        // 먼저 페이드 인
        yield return StartCoroutine(FadeCanvasGroup(fadeGroup, 0f, 1f));

        // 그 후 게임 정지
        Time.timeScale = 0f;
    }

    IEnumerator CloseInventory()
    {
        // 먼저 게임 다시 시작
        Time.timeScale = 1f;

        // 페이드 아웃
        yield return StartCoroutine(FadeCanvasGroup(fadeGroup, 1f, 0f));

        // 인벤토리 비활성화
        inventoryPanel.SetActive(false);
        isInventoryOpen = false;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            yield return null;
        }
        cg.alpha = end;
        cg.blocksRaycasts = end > 0f;
        cg.interactable = end > 0f;
    }
}
