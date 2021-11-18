using System;

namespace OFX.RAASManager.Services
{
    public interface IDayLightSavingService
    {
        bool IsNewYorkOnDayLightSaving(DateTime date);

        bool IsNewZealandOnDayLightSaving(DateTime date);
    }
}