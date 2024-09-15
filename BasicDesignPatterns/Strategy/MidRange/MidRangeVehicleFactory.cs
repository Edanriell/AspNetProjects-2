namespace Vehicles.MidRange;

public class MidRangeVehicleFactory : IVehicleFactory
{
	public IBike CreateBike()
	{
		return new MidRangeBike();
	}

	public ICar CreateCar()
	{
		return new MidRangeCar();
	}
}