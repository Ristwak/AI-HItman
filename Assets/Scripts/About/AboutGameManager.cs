using UnityEngine;
using TMPro;

/// <summary>
/// Populates the About panel with det,vkbZled game info for OptiMind (multi-correct logic game).
/// </summary>
public class AboutGameManager : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Assign the TMP_Text component for the About section.")]
    public TMP_Text aboutText;

    void Start()
    {
        if (aboutText == null)
        {
            Debug.LogError("AboutGameManager: 'aboutText' reference is missing.");
            return;
        }

        aboutText.text =
            "<b>v‚fIVekbaM ds ckjs esa</b>\n" +
            "v‚fIVekbaM ,d eYVh&flysDV jhtfuax pkSysat gS] ftls ,vkbZ] y‚ftd vkSj e'khu yfuZax dh vkidh voèkkj.kkRed le> dks rst djus ds fy, fMt+kbu fd;k x;k gSA\n\n" +

            "<b>;g xse D;k gS\\</b>\n" +
            "gj Lrj ij vkidks tfVy] okLrfod&thou ls çsfjr ç'u feysaxs ftuds ,d ls vfèkd lgh mÙkj gks ldrs gSaA\n" +
            "vkidk dke: lHkh oSèk mÙkjksa dk fo'ys\"k.k djuk] mUgsa igpkuuk vkSj pquuk — flQZ Li\"V ugha] cfYd lHkh mi;qä fodYiA\n\n" +

            "<b>A ;k B ls vkxs dh lksp</b>\n" +
            "v‚fIVekbaM mu yksxksa dks iqjL—r djrk gS tks Li\"V lksp] iSVuZ dh igpku vkSj ,d lkFk dbZ laHkkoukvksa dks le>us dh {kerk j[krs gSaA\n" +
            ";g ,e ,yh çdkj dh cgq&iFk fu.kZ; {kerk dks n'kkZrk gS ftldh t:jr ,vkbZ fMt+kbu vkSj ewY;kadu esa gksrh gSA\n\n" +

            "<b>vki D;k lh[ksaxs</b>\n" +
            "• vLi\"V vkSj vewrZ leL;kvksa ls dSls fuiVsa\n" +
            "• tgk¡ ikj,vkbZfjd y‚ftd vkSj laHkkO;kRed rdZ ,d&nwljs ls feyrs gSa\n" +
            "• dsoy lcls vPNk ugha] cfYd lHkh lgh mÙkjksa dks pquus dh pqukSrh\n\n" +

            "<b>,vkbZ@,e ,y Nk=ksa ds fy, fMt+kbu fd;k x;k</b>\n" +
            ",vkbZ] ,e ,y vkSj d‚fXufVo lkbal ds Nk=ksa vkSj mRlkgh yksxksa ds fy, cuk;k x;k] v‚fIVekbaM vkidh enn djrk gS:\n" +
            "• y‚ftdy QkmaMs'kUl dks etcwr cukuk\n" +
            "• dbZ fodYiksa ds chp rdZ ykxw djuk\n" +
            "• Q‚Yl i‚ftfVOl dh igpku djuk vkSj Hkzked fodYiksa dks gVkuk\n\n" +

            "<b>D;k vki vyx lkspus dks rS;kj gSa\\</b>\n" +
            ";g vuqeku ugha] cfYd ncko esa xgjh le> gSA\n" +
            "tkapsaA pqusaA fopkj djsaA\n\n" +
            "<b>v‚fIVekbaM esa vkidk Lokxr gSA<\b>";
    }
}
