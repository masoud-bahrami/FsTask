using FsTask.AcceptanceTests.Helpers;
using FsTask.ApplicationServices;
using FsTask.Domain.Contract;

namespace FsTask.AcceptanceTests.StepDefinitions.Drivers;

public static class SensorDataAbstractFactory
{
    private const string Human1 = "human1";
    private const string Human2 = "human2";
    private const string Human3 = "human3";

    public static StoreSensorDataCommand CreateCommand(Table eventTable)
    {
        return StoreSensorDataCommandBuilder.INeed()
            .WithInstance(InstanceDtoBuilder.INeedANewInsatnce()
                .WithHumanId(eventTable.Get(0, SensorTableHeaders.HumanId))
                .WithPositionX(double.Parse(eventTable.Get(0, SensorTableHeaders.PositionX)))
                .WithPositionY(double.Parse(eventTable.Get(0, SensorTableHeaders.PositionY)))
                .WithVelX(int.Parse(eventTable.Get(0, SensorTableHeaders.VelX)))
                .WithVelY(int.Parse(eventTable.Get(0, SensorTableHeaders.VelY)))
                .ThankYou())
            .ThankYou();
    }
    public static StoreSensorDataCommand CreateCommand()
    {
        return StoreSensorDataCommandBuilder.INeed()
            .WithInstance(InstanceDtoBuilder.INeedANewInsatnce().ThankYou())
            .ThankYou();
    }


    public static SensorDataViewModel Create(Table eventTable)
    {
        return new SensorDataViewModel
        {
            TimeStamp = eventTable.Get(0, SensorTableHeaders.TimeStamp),
            Instances = new List<InstanceViewModel>
            {
                new()
                {
                    HumanId = eventTable.Get(0, SensorTableHeaders.HumanId),
                    PositionX =double.Parse(eventTable.Get(0, SensorTableHeaders.PositionX)),
                    PositionY = double.Parse(eventTable.Get(0, SensorTableHeaders.PositionY)),
                    VelX = int.Parse(eventTable.Get(0, SensorTableHeaders.VelX)),
                    VelY = int.Parse(eventTable.Get(0, SensorTableHeaders.VelY)),
                }
            }
        };
    }



    public static StoreSensorDataCommand Create(TableRow row)
    {
        var instanceViewModels = new List<InstanceDto>();

        if (Human1Detected(row))
            instanceViewModels.Add(InstanceDtoBuilder.INeedANewInsatnce().WithHumanId(Human1).ThankYou());
        if (Human2Detected(row))
            instanceViewModels.Add(InstanceDtoBuilder.INeedANewInsatnce().WithHumanId(Human2).ThankYou());
        if (Human3Detected(row))
            instanceViewModels.Add(InstanceDtoBuilder.INeedANewInsatnce().WithHumanId(Human3).ThankYou());

        return new StoreSensorDataCommand
        {
            Instances = instanceViewModels
        };
    }
    public static StoreSensorDataCommand CreatePosX(TableRow row)
    {
        var instanceViewModels = new List<InstanceDto>();

        if (IsPositionXOfHuman1Detected(row))
        {
            instanceViewModels.Add(InstanceDtoBuilder.INeedANewInsatnce().WithHumanId(Human1)
                .WithPositionX(PositionX(1, row))
                .ThankYou());
        }

        if (IsPositionXOfHuman2Detected(row))
            instanceViewModels.Add(InstanceDtoBuilder.INeedANewInsatnce().WithHumanId(Human2)
                .WithPositionX(PositionX(2, row))
                .ThankYou());
        if (IsPositionXOfHuman3Detected(row))
            instanceViewModels.Add(InstanceDtoBuilder.INeedANewInsatnce().WithHumanId(Human3)
                .WithPositionX(PositionX(3, row)).ThankYou());

        return new StoreSensorDataCommand
        {
            Instances = instanceViewModels
        };
    }

    public static StoreSensorDataCommand CreatePosY(TableRow row)
    {
        var instanceViewModels = new List<InstanceDto>();

        if (IsPositionYOfHuman1Detected(row))
        {
            instanceViewModels.Add(InstanceDtoBuilder.INeedANewInsatnce().WithHumanId(Human1)
                .WithPositionY(PositionX(1, row))
                .ThankYou());
        }

        if (IsPositionYOfHuman2Detected(row))
            instanceViewModels.Add(InstanceDtoBuilder.INeedANewInsatnce().WithHumanId(Human2)
                .WithPositionY(PositionX(2, row))
                .ThankYou());
        if (IsPositionYOfHuman3Detected(row))
            instanceViewModels.Add(InstanceDtoBuilder.INeedANewInsatnce().WithHumanId(Human3)
                .WithPositionY(PositionX(3, row)).ThankYou());

        return new StoreSensorDataCommand
        {
            Instances = instanceViewModels
        };
    }

    private static double PositionX(int index, TableRow row)
    {
        var remove = row[index].Remove(0, SensorTableHeaders.PositionDetected.Length);
        return double.Parse(GetXPosition(remove));
    }

    private static string GetXPosition(string remove)
    {
        return remove.Trim().Split('-')[0];
    }

    public static DashboardReportViewModel CreateDashboardReportViewModel(TableRows rows)
    {
        return new DashboardReportViewModel
        {
            Items = rows.Select(AddInstanceItem).ToList()

        };
    }


    public static DashboardReportViewModel CreateDashboardReportViewModelPosX(TableRows rows)
    {
        return new DashboardReportViewModel
        {
            Items = rows.Select(DashboardReportViewModelItem).ToList()
        };
    }

    public static DashboardReportViewModel CreateDashboardReportViewModelPosY(TableRows rows)
    {
        return new DashboardReportViewModel
        {
            Items = rows.Select(DashboardReportViewModelItem).ToList()

        };
    }
    private static DashboardReportViewModelItem DashboardReportViewModelItem(TableRow row)
    {
        var yAxis = new List<string>();
        if (!string.IsNullOrWhiteSpace(row[1])) yAxis.Add(row[1]);
        if (!string.IsNullOrWhiteSpace(row[2])) yAxis.Add(row[2]);
        if (!string.IsNullOrWhiteSpace(row[3])) yAxis.Add(row[3]);


        return new DashboardReportViewModelItem
        {
            Timestamp = long.Parse(row[SensorTableHeaders.TimeStamp]),
            YAxisDatas = yAxis
        };
    }


    private static DashboardReportViewModelItem AddInstanceItem(TableRow row)
    {
        var yAxis = new List<string>();

        if (Human1Detected(row)) yAxis.Add(Human1);
        if (Human2Detected(row)) yAxis.Add(Human2);
        if (Human3Detected(row)) yAxis.Add(Human3);

        return new DashboardReportViewModelItem
        {
            Timestamp = long.Parse(row[SensorTableHeaders.TimeStamp]),
            YAxisDatas = yAxis
        };
    }

    static bool IsPositionYOfHuman1Detected(TableRow row)
        => IsPositionYDetected(1, row);

    static bool IsPositionYOfHuman2Detected(TableRow row)
        => IsPositionYDetected(2, row);

    static bool IsPositionYOfHuman3Detected(TableRow row)
        => IsPositionYDetected(3, row);

    static bool IsPositionYDetected(int atIndex, TableRow row)
        => row[atIndex].StartsWith(SensorTableHeaders.PositionDetected);

    static bool IsPositionXOfHuman1Detected(TableRow row)
        => IsPositionXDetected(1, row);

    static bool IsPositionXOfHuman2Detected(TableRow row)
        => IsPositionXDetected(2, row);

    static bool IsPositionXOfHuman3Detected(TableRow row)
    => IsPositionXDetected(3, row);

    static bool IsPositionXDetected(int atIndex, TableRow row)
        => row[atIndex].StartsWith(SensorTableHeaders.PositionDetected);

    static bool Human1Detected(TableRow row)
    {
        return HumanDetectedAt(1, row);
    }
    static bool Human2Detected(TableRow row)
    {
        return HumanDetectedAt(2, row);
    }
    static bool Human3Detected(TableRow row)
    {
        return HumanDetectedAt(3, row);
    }

    static bool HumanDetectedAt(int at, TableRow row)
    {
        return row[at] == SensorTableHeaders.Detected;
    }
}