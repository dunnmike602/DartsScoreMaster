using System;
using Windows.UI.Xaml.Media.Imaging;
using DartsScoreMaster.Model;
using ReactiveUI;

namespace DartsScoreMaster.Common
{
    public static class StandingData
    {
        public static ReactiveList<Flight> _flights;

        public static ReactiveList<Flight> GetFlights()
        {
            if (_flights != null)
            {
                return _flights;
            }

            _flights = new ReactiveList<Flight>();

            for (var i = 1; i <= 6; i++)
            {
                var flight = new Flight
                {
                    Index = i,
                    Image = new BitmapImage(new Uri($"ms-appx:///Assets/scoredart{i}.png", UriKind.Absolute))
                };

                _flights.Add(flight);
            }

            return _flights;
        }
    }
}