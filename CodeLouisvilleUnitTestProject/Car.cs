using Newtonsoft.Json;

namespace CodeLouisvilleUnitTestProject
{
    internal class Car : Vehicle
    {
        public int NumberOfPassengers { get; private set; }
        private static HttpClient _client = new ()
        {
            BaseAddress = new Uri("https://vpic.nhtsa.dot.gov/api/vehicles/")
        };

        public Car() : base(4, 0, "", "", 0)
        { }

        public Car(double gasTankCapacity, string make, string model, double milesPerGallon) : base(4, gasTankCapacity, make, model, milesPerGallon)
        {

        }

        public async Task<bool> IsValidModelForMakeAsync()
        {
            String endPoint = _client.BaseAddress + "GetModelsForMake/" + Make + "?format=json";
            HttpResponseMessage response = await _client.GetAsync(endPoint);
            String responseBody = await response.Content.ReadAsStringAsync();
            ModelForMakeDetails makeDetails = JsonConvert.DeserializeObject<ModelForMakeDetails>(responseBody);
            foreach (ModelDetails details in makeDetails.Results)
            {
                if (details.Model_Name.Equals(Model))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> WasModelMadeInYearAsync(int year)
        {
            if (year < 1995)
            {
                throw new ArgumentException("no data is available for years before 1995");
            }
            else
            {
                String endPoint = _client.BaseAddress + "GetModelsForMakeYear/make/" + Make;
                endPoint += "/modelyear/" + year;
                endPoint += "/vehicletype/car?format=json";

                HttpResponseMessage response = await _client.GetAsync(endPoint);
                String responseBody = await response.Content.ReadAsStringAsync();
                ModelForMakeDetails makeDetails = JsonConvert.DeserializeObject<ModelForMakeDetails>(responseBody);

                foreach (ModelDetails details in makeDetails.Results)
                {
                    if (details.Model_Name.Equals(Model))
                    {
                        return true;
                    }
                }
                return false;
            }
            
        }

        public void AddPassengers(int passengersToAdd)
        {
            NumberOfPassengers += passengersToAdd;
            MilesPerGallon -= (double)(passengersToAdd * 0.2);
        }

        public void RemovePassengers(int passengersToRemove)
        {
            if ((NumberOfPassengers - passengersToRemove) >= 0)
            {
                NumberOfPassengers -= passengersToRemove;
                MilesPerGallon += (double)(passengersToRemove * 0.2);
            } 
            else if (NumberOfPassengers > 0 && (NumberOfPassengers - passengersToRemove) <= 0)
            {
                MilesPerGallon += (double)(NumberOfPassengers * 0.2);
                NumberOfPassengers = 0;
            }
            
        }
    }
}
