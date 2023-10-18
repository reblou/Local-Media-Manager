using System.Collections.Generic;

namespace MyFlix
{
    internal class UserMediaSaveData
    {
        public List<Film> films;
        public List<TVSeries> series;

        public UserMediaSaveData(List<IDisplayable> displayables)
        {
            films = new List<Film>();
            series = new List<TVSeries>();

            if (displayables == null || displayables.Count == 0) return;

            foreach (IDisplayable diplayable in displayables)
            {
            if (diplayable is Film)
            {
                films.Add((Film)diplayable);
            }
            else if (diplayable is TVSeries) 
            { 
                series.Add((TVSeries)diplayable);
            }
            }
        }

        public List<IDisplayable> GetCombinedList()
        {
            List<IDisplayable> displayables = new List<IDisplayable>();
            displayables.AddRange(films);
            displayables.AddRange(series);
            return displayables;
        }
    }
}
