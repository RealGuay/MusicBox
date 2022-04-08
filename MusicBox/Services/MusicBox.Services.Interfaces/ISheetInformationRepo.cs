using System;
using System.Collections.Generic;
using System.Text;
using MusicBox.Services.Interfaces.MusicSheetModels;


namespace MusicBox.Services.Interfaces
{
    public  interface ISheetInformationRepo
    {
        void Load(SheetInformation sheetInformation);
        void Save(SheetInformation sheetInformation);
    }
}
