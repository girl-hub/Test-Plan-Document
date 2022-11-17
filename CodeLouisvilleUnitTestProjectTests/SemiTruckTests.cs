using CodeLouisvilleUnitTestProject;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit.Sdk;

namespace CodeLouisvilleUnitTestProjectTests
{
    public class SemiTruckTests
    {
        public SemiTruck st;
        public List<CargoItem> cargo;
        public SemiTruckTests()
        {
            cargo = new List<CargoItem>();
            CargoItem cargoItem1 = new()
            {
                Quantity = 5,
                Name = "cargo",
                Description = "cargoDescription1"
            };
            CargoItem cargoItem2 = new()
            {
                Quantity = 10,
                Name = "cargo23",
                Description = "cargoDescription2"
            };
            CargoItem cargoItem3 = new()
            {
                Quantity = 15,
                Name = "cargo",
                Description = "cargoDescription3"
            };
            cargo.Add(cargoItem1);
            cargo.Add(cargoItem2);
            cargo.Add(cargoItem3);

            st = new SemiTruck();
        }

        //Verify that the SemiTruck constructor creates a new SemiTruck
        //object which is also a Vehicle and has 18 wheels. Verify that the
        //Cargo property for the newly created SemiTruck is a List of
        //CargoItems which is empty, but not null.
        [Fact]
        public void NewSemiTruckIsAVehicleAndHas18TiresAndEmptyCargoTest()
        {
            //assert
            st.NumberOfTires.Should().Be(18);
            st.Cargo.Should().NotBeNull();
            st.Cargo.Count.Should().Be(0);

        }

        //Verify that adding a CargoItem using LoadCargo does successfully add
        //that CargoItem to the Cargo. Confirm both the existence of the new
        //CargoItem in the Cargo and also that the count of Cargo increased to 1.
        [Fact]
        public void LoadCargoTest()
        {
            st.LoadCargo(cargo[0]);

            st.Cargo.Count.Should().Be(1);
            st.Cargo[0].Should().Be(cargo[0]);

        }

        //Verify that unloading a  cargo item that is in the Cargo does
        //remove it from the Cargo and return the matching CargoItem
        [Fact]
        public void UnloadCargoWithValidCargoTest()
        {
            st.LoadCargo(cargo[0]);

            st.UnloadCargo("cargo").Should().Be(cargo[0]);
            st.Cargo.Count.Should().Be(0);
        }

        //Verify that attempting to unload a CargoItem that does not
        //appear in the Cargo throws a System.ArgumentException
        [Fact]
        public void UnloadCargoWithInvalidCargoTest()
        {
            st.LoadCargo(cargo[0]);
            Action action = () => st.UnloadCargo("invalidCargoName");

            action.Should().Throw<ArgumentException>();
            st.Cargo.Count.Should().Be(1);
        }

        //Verify that getting cargo items by name returns all items
        //in Cargo with that name.
        [Fact]
        public void GetCargoItemsByNameWithValidName()
        {
            st.LoadCargo(cargo[0]);
            st.LoadCargo(cargo[1]);
            st.LoadCargo(cargo[2]);

            st.GetCargoItemsByName("cargo").Count.Should().Be(2);
        }

        //Verify that searching the Carto list for an item that does not
        //exist returns an empty list
        [Fact]
        public void GetCargoItemsByNameWithInvalidName()
        {
            st.LoadCargo(cargo[0]);
            st.LoadCargo(cargo[1]);
            st.LoadCargo(cargo[2]);

            st.GetCargoItemsByName("cargo123").Count.Should().Be(0);
        }

        //Verify that searching the Cargo list by description for an item
        //that does exist returns all matched items that contain that description.
        [Fact]
        public void GetCargoItemsByPartialDescriptionWithValidDescription()
        {
            st.LoadCargo(cargo[0]);
            st.LoadCargo(cargo[1]);
            st.LoadCargo(cargo[2]);

            st.GetCargoItemsByPartialDescription("cargoDescription").Count.Should().Be(3);
        }

        //Verify that searching the Carto list by description for an item
        //that does not exist returns an empty list
        [Fact]
        public void GetCargoItemsByPartialDescriptionWithInvalidDescription()
        {
            st.LoadCargo(cargo[0]);
            st.LoadCargo(cargo[1]);
            st.LoadCargo(cargo[2]);

            st.GetCargoItemsByPartialDescription("cargoDescription does not exist").Count.Should().Be(0);
        }

        //Verify that the method returns the sum of all quantities of all
        //items in the Cargo
        [Fact]
        public void GetTotalNumberOfItemsReturnsSumOfAllQuantities()
        {
            st.LoadCargo(cargo[0]);
            st.LoadCargo(cargo[1]);
            st.LoadCargo(cargo[2]);

            st.GetTotalNumberOfItems().Should().Be(30);
        }
    }
}
