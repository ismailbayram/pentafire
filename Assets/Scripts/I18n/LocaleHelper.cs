using UnityEngine;
using System.Collections.Generic;

public static class LocaleHelper {
    public static Dictionary<string, Dictionary<string, string>> Texts = new Dictionary<string, Dictionary<string, string>>{
        {"EN", new Dictionary<string, string>{
            {"play", "PLAY"},
            {"best_score", "BEST SCORE: "},
            {"tips_title", "TIPS"},
            {"tips", "If your energy is less than 50, you will die!\n\nYou should collect the energies to gain energy and point!\n\nSometimes enemies drop some modules which give you extra abilities."},
            {"new_best", "NEW BEST!"},
            {"continue", "Continue"},
            {"score", "SCORE:"},
            {"revive", "REVIVE"},
            {"you_are_dead", "YOU ARE DEAD"},
            {"double_kill", "DOUBLE KILL!"},
            {"triple_kill", "TRIPLE KILL!"},
            {"quadra_kill", "QUADRA KILL!"},
            {"penta_kill", "PENTA KILL!"},
            {"master_kill", "MASTER KILL!"},
            {"paused", "PAUSED"},
            {"restart", "Restart"},
            {"reflector", "REFLECTOR"},
            {"speed", "SPEED"},
            {"balls", "BALLS"},
        }},
        {"TR", new Dictionary<string, string>{
            {"play", "OYNA"},
            {"best_score", "EN YÜKSEK PUAN: "},
            {"tips_title", "İP UÇLARI"},
            {"tips", "Enerjiniz 50'den az ise ölürsünüz.\n\nEnerji ve puan kazanmak için yerdeki enerjileri toplamalısınız.\n\nBazen düşmanlar ek yetenekler kazanabileceğiniz modüller düşürür."},
            {"new_best", "YENİ REKOR!"},
            {"continue", "Devam et"},
            {"score", "PUAN:"},
            {"revive", "CANLAN"},
            {"you_are_dead", "ÖLDÜN"},
            {"double_kill", "İKİDE Kİ!"},
            {"triple_kill", "ÜÇTE ÜÇ!"},
            {"quadra_kill", "DÖRTTE DÖRT!"},
            {"penta_kill", "BEŞTE BEŞ!"},
            {"master_kill", "KATLİAM!"},
            {"paused", "DURAKLATILDI"},
            {"restart", "Tekrar Dene"},
            {"reflector", "KALKAN"},
            {"speed", "HIZLI"},
            {"balls", "TOPLAR"},
        }}
    };

    public static string GetLang() {
        SystemLanguage lang = Application.systemLanguage;

        switch (lang) {
            case SystemLanguage.Turkish:
                return "TR";
            default:
                return GetDefaultSupportedLanguageCode();
        }
    }

    public static string GetDefaultSupportedLanguageCode() {
        return "EN";
    }

    public static string GetText(string key) {
        return Texts[GetLang()][key];
    }
}