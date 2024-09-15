namespace Vehicles.HighEnd;

public class HighEndVehicleFactory : IVehicleFactory
{
	public IBike CreateBike()
	{
		return new HighEndBike();
	}

	public ICar CreateCar()
	{
		return new HighEndCar();
	}
}