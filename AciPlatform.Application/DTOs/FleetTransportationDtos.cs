namespace AciPlatform.Application.DTOs;

public class CarModel
{
    public int Id { get; set; }
    public string? LicensePlates { get; set; }
    public string? Note { get; set; }
    public string? Content { get; set; }
    public double MileageAllowance { get; set; }
    public double FuelAmount { get; set; }
    public int? CarFleetId { get; set; }
    public List<string>? Files { get; set; }
}

public class CarFleetModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CarFleetPagingModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CarCount { get; set; }
}

public class CarGetterModel
{
    public int Id { get; set; }
    public string? LicensePlates { get; set; }
    public string? Note { get; set; }
    public int? CarFleetId { get; set; }
}

public class CarGetterPagingModel
{
    public int Id { get; set; }
    public string? LicensePlates { get; set; }
    public string? Note { get; set; }
    public double MileageAllowance { get; set; }
    public double FuelAmount { get; set; }
    public int? CarFleetId { get; set; }
}

public class CarGetterDetailModel
{
    public int Id { get; set; }
    public string? LicensePlates { get; set; }
    public string? Note { get; set; }
    public string? Content { get; set; }
    public double MileageAllowance { get; set; }
    public double FuelAmount { get; set; }
    public int? CarFleetId { get; set; }
    public List<string>? Files { get; set; }
}

public class CarFieldModel
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int Order { get; set; }
    public string? Name { get; set; }
}

public class CarFieldPagingModel
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int Order { get; set; }
    public string? Name { get; set; }
}

public class CarFieldSetupModel
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int CarFieldId { get; set; }
    public double? ValueNumber { get; set; }
    public DateTime? FromAt { get; set; }
    public DateTime? ToAt { get; set; }
    public DateTime? WarningAt { get; set; }
    public string? UserIdString { get; set; }
    public string? Note { get; set; }
    public string? FileLink { get; set; }
}

public class CarFieldSetupGetterModel
{
    public int CarFieldId { get; set; }
    public string? Name { get; set; }
    public int Order { get; set; }
    public double? ValueNumber { get; set; }
    public DateTime? FromAt { get; set; }
    public DateTime? ToAt { get; set; }
    public DateTime? WarningAt { get; set; }
    public string? UserIdString { get; set; }
    public string? Note { get; set; }
    public string? FileLink { get; set; }
}

public class CarLocationModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public string? ProcedureNumber { get; set; }
    public string? Status { get; set; }
    public List<CarLocationDetailModel>? Details { get; set; }
}

public class CarLocationDetailModel
{
    public int Id { get; set; }
    public int CarLocationId { get; set; }
    public string? LicensePlates { get; set; }
    public string? Type { get; set; }
    public string? Payload { get; set; }
    public string? DriverName { get; set; }
    public string? Location { get; set; }
    public string? PlanInprogress { get; set; }
    public string? PlanExpected { get; set; }
    public string? Note { get; set; }
    public string? FileStr { get; set; }
}

public class CarLocationPagingModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public string? ProcedureNumber { get; set; }
    public string? Status { get; set; }
}

public class DriverRouterModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }
    public int PetrolConsumptionId { get; set; }
    public double? AdvancePaymentAmount { get; set; }
    public double? FuelAmount { get; set; }
}

public class DriverRouterPagingModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }
    public int PetrolConsumptionId { get; set; }
    public double? AdvancePaymentAmount { get; set; }
    public double? FuelAmount { get; set; }
    public string? RoadRouteName { get; set; }
    public string? LicensePlates { get; set; }
    public string? Driver { get; set; }
    public double KmFrom { get; set; }
    public double KmTo { get; set; }
}

public class PoliceCheckPointModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public double Amount { get; set; }
}

public class PetrolConsumptionModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public int CarId { get; set; }
    public double PetroPrice { get; set; }
    public double KmFrom { get; set; }
    public double KmTo { get; set; }
    public string? LocationFrom { get; set; }
    public string? LocationTo { get; set; }
    public double AdvanceAmount { get; set; }
    public string? Note { get; set; }
    public int? RoadRouteId { get; set; }
    public List<PetrolConsumptionPoliceCheckPointModel>? Points { get; set; }
}

public class PetrolConsumptionGetterModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public int CarId { get; set; }
    public double PetroPrice { get; set; }
    public double KmFrom { get; set; }
    public double KmTo { get; set; }
    public string? LocationFrom { get; set; }
    public string? LocationTo { get; set; }
    public double AdvanceAmount { get; set; }
    public string? Note { get; set; }
    public int? RoadRouteId { get; set; }
}

public class PetrolConsumptionPoliceCheckPointModel
{
    public double Amount { get; set; }
    public string? PoliceCheckPointName { get; set; }
    public bool IsArise { get; set; }
}

public class PetrolConsumptionReportRequestModel
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? CarId { get; set; }
    public int? UserId { get; set; }
}

public class PetrolConsumptionReportModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int CarId { get; set; }
    public int UserId { get; set; }
    public double KmFrom { get; set; }
    public double KmTo { get; set; }
    public string? LocationFrom { get; set; }
    public string? LocationTo { get; set; }
    public double AdvanceAmount { get; set; }
    public double PetroPrice { get; set; }
    public string? Note { get; set; }
    public int? RoadRouteId { get; set; }
    public string? LicensePlates { get; set; }
    public string? DriverName { get; set; }
    public string? RoadRouteName { get; set; }
    public double TotalPoliceCheckPointAmount { get; set; }
}

public class RoadRouteModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? RoadRouteDetail { get; set; }
    public string? PoliceCheckPointIdStr { get; set; }
    public double NumberOfTrips { get; set; }
}

public class RoadRoutePagingModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? RoadRouteDetail { get; set; }
    public string? PoliceCheckPointIdStr { get; set; }
    public double NumberOfTrips { get; set; }
}
