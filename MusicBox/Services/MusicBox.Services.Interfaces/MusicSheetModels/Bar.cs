using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class Bar
    {
        public int Id { get; set; }
        public int PlayOrder { get; set; }
        public List<SheetNote> SheetNotes { get; set; }

        public Bar()
        {
            SheetNotes = new List<SheetNote>();
        }

        //internal string TraceDetails()
        //{
        //    string barDetails = $"Bar Id : {Id} PlayOrder : {PlayOrder} \n";
        //    foreach (SheetNote note in SheetNotes)
        //    {
        //        barDetails += note.TraceDetails();
        //    }
        //    return barDetails;
        //}
    }
}
