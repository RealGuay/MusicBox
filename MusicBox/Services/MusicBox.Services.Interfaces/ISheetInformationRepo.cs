using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicBox.Services.Interfaces.MusicSheetModels;


namespace MusicBox.Services.Interfaces
{
    public  interface ISheetInformationRepo
    {
        Task LoadAsync(SheetInformation sheetInformation);
        Task SaveAsync(SheetInformation sheetInformation);
    }
}
