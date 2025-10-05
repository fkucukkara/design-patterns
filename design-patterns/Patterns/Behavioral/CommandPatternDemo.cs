using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Behavioral.Command;

/// <summary>
/// Demonstrates the Command pattern with a smart home automation system.
/// The Command pattern encapsulates a request as an object, allowing you to parameterize
/// clients with different requests, queue operations, and support undo functionality.
/// </summary>
public class CommandPatternDemo : IPatternDemo
{
    public string PatternName => "Command";

    public string Description => "Encapsulates a request as an object, allowing you to parameterize clients with different requests, " +
                               "queue or log requests, and support undo operations. Useful for implementing macro commands, " +
                               "undo/redo functionality, and decoupling the invoker from the receiver.";

    public void Demonstrate()
    {
        Console.WriteLine("🏠 Smart Home Automation Command Example");
        Console.WriteLine();

        DemonstrateBasicCommands();
        Console.WriteLine();
        
        DemonstrateMacroCommands();
        Console.WriteLine();
        
        DemonstrateUndoFunctionality();
        Console.WriteLine();
        
        DemonstrateCommandQueue();
    }

    private static void DemonstrateBasicCommands()
    {
        Console.WriteLine("💡 Basic Device Commands:");
        
        // Create devices (receivers)
        var livingRoomLight = new SmartLight("Living Room");
        var kitchenLight = new SmartLight("Kitchen");
        var thermostat = new SmartThermostat("Main");
        var musicSystem = new MusicSystem("Sonos");
        
        // Create remote control (invoker)
        var remote = new SmartHomeRemote();
        
        // Create and execute commands
        var commands = new List<ICommand>
        {
            new LightOnCommand(livingRoomLight),
            new LightOffCommand(kitchenLight),
            new SetTemperatureCommand(thermostat, 72),
            new PlayMusicCommand(musicSystem, "Classical Playlist"),
            new SetVolumeCommand(musicSystem, 60)
        };

        foreach (var command in commands)
        {
            remote.SetCommand(command);
            remote.PressButton();
        }
    }

    private static void DemonstrateMacroCommands()
    {
        Console.WriteLine("🎭 Macro Commands (Multiple Actions):");
        
        var devices = CreateDevices();
        var remote = new SmartHomeRemote();
        
        // Create a "Movie Night" macro command
        var movieNightCommands = new List<ICommand>
        {
            new LightOffCommand(devices.LivingRoomLight),
            new LightOnCommand(devices.KitchenLight, 20), // Dim setting
            new SetTemperatureCommand(devices.Thermostat, 68),
            new PlayMusicCommand(devices.MusicSystem, "Movie Soundtracks"),
            new SetVolumeCommand(devices.MusicSystem, 40)
        };
        
        var movieNightMacro = new MacroCommand("Movie Night", movieNightCommands);
        
        Console.WriteLine("  🍿 Executing 'Movie Night' macro...");
        remote.SetCommand(movieNightMacro);
        remote.PressButton();
        
        Console.WriteLine();
        
        // Create a "Good Morning" macro command
        var goodMorningCommands = new List<ICommand>
        {
            new LightOnCommand(devices.LivingRoomLight, 80),
            new LightOnCommand(devices.KitchenLight, 100),
            new SetTemperatureCommand(devices.Thermostat, 70),
            new PlayMusicCommand(devices.MusicSystem, "Morning Jazz"),
            new SetVolumeCommand(devices.MusicSystem, 50)
        };
        
        var goodMorningMacro = new MacroCommand("Good Morning", goodMorningCommands);
        
        Console.WriteLine("  ☀️ Executing 'Good Morning' macro...");
        remote.SetCommand(goodMorningMacro);
        remote.PressButton();
    }

    private static void DemonstrateUndoFunctionality()
    {
        Console.WriteLine("↩️ Undo Functionality:");
        
        var light = new SmartLight("Bedroom");
        var remote = new SmartHomeRemote();
        
        // Execute a series of commands with undo capability
        var commands = new ICommand[]
        {
            new LightOnCommand(light),
            new LightOffCommand(light),
            new LightOnCommand(light, 50),
            new LightOnCommand(light, 100)
        };

        Console.WriteLine("  🔄 Executing commands:");
        foreach (var command in commands)
        {
            remote.SetCommand(command);
            remote.PressButton();
        }
        
        Console.WriteLine("\n  ↶ Undoing last 3 commands:");
        for (int i = 0; i < 3; i++)
        {
            remote.PressUndo();
        }
    }

    private static void DemonstrateCommandQueue()
    {
        Console.WriteLine("⏰ Scheduled Command Queue:");
        
        var devices = CreateDevices();
        var scheduler = new CommandScheduler();
        
        // Schedule commands for different times
        scheduler.ScheduleCommand(
            new LightOnCommand(devices.LivingRoomLight), 
            DateTime.Now.AddSeconds(1), 
            "Morning lights");
            
        scheduler.ScheduleCommand(
            new SetTemperatureCommand(devices.Thermostat, 68), 
            DateTime.Now.AddSeconds(2), 
            "Lower temperature");
            
        scheduler.ScheduleCommand(
            new PlayMusicCommand(devices.MusicSystem, "Wake Up Playlist"), 
            DateTime.Now.AddSeconds(3), 
            "Start music");

        Console.WriteLine("  📅 Commands scheduled. Executing queue...");
        scheduler.ExecuteScheduledCommands();
    }

    private static (SmartLight LivingRoomLight, SmartLight KitchenLight, SmartThermostat Thermostat, MusicSystem MusicSystem) CreateDevices()
    {
        return (
            LivingRoomLight: new SmartLight("Living Room"),
            KitchenLight: new SmartLight("Kitchen"),
            Thermostat: new SmartThermostat("Main"),
            MusicSystem: new MusicSystem("Home Audio")
        );
    }
}

// Command interface
/// <summary>
/// The command interface that all concrete commands implement.
/// </summary>
public interface ICommand
{
    void Execute();
    void Undo();
    string GetDescription();
}

// Receiver classes (the objects that perform the actual work)
/// <summary>
/// Smart light device that can be controlled via commands.
/// </summary>
public class SmartLight
{
    public string Location { get; }
    public bool IsOn { get; private set; }
    public int Brightness { get; private set; } = 100;

    public SmartLight(string location)
    {
        Location = location;
    }

    public void TurnOn(int brightness = 100)
    {
        IsOn = true;
        Brightness = brightness;
        Console.WriteLine($"    💡 {Location} light turned ON (brightness: {Brightness}%)");
    }

    public void TurnOff()
    {
        IsOn = false;
        Console.WriteLine($"    🌙 {Location} light turned OFF");
    }

    public void SetBrightness(int brightness)
    {
        if (IsOn)
        {
            Brightness = Math.Clamp(brightness, 0, 100);
            Console.WriteLine($"    🔆 {Location} light brightness set to {Brightness}%");
        }
    }

    public (bool IsOn, int Brightness) GetState() => (IsOn, Brightness);
}

/// <summary>
/// Smart thermostat that can be controlled via commands.
/// </summary>
public class SmartThermostat
{
    public string Zone { get; }
    public int Temperature { get; private set; } = 70;

    public SmartThermostat(string zone)
    {
        Zone = zone;
    }

    public void SetTemperature(int temperature)
    {
        var oldTemp = Temperature;
        Temperature = Math.Clamp(temperature, 50, 85);
        
        var trend = Temperature > oldTemp ? "🔥" : Temperature < oldTemp ? "❄️" : "➡️";
        Console.WriteLine($"    🌡️ {Zone} thermostat set to {Temperature}°F {trend}");
    }

    public int GetTemperature() => Temperature;
}

/// <summary>
/// Music system that can be controlled via commands.
/// </summary>
public class MusicSystem
{
    public string Name { get; }
    public bool IsPlaying { get; private set; }
    public string? CurrentPlaylist { get; private set; }
    public int Volume { get; private set; } = 50;

    public MusicSystem(string name)
    {
        Name = name;
    }

    public void Play(string playlist)
    {
        IsPlaying = true;
        CurrentPlaylist = playlist;
        Console.WriteLine($"    🎵 {Name} playing: {playlist}");
    }

    public void Stop()
    {
        IsPlaying = false;
        Console.WriteLine($"    ⏹️ {Name} stopped");
    }

    public void SetVolume(int volume)
    {
        Volume = Math.Clamp(volume, 0, 100);
        Console.WriteLine($"    🔊 {Name} volume set to {Volume}%");
    }

    public (bool IsPlaying, string? Playlist, int Volume) GetState() => (IsPlaying, CurrentPlaylist, Volume);
}

// Concrete command classes
/// <summary>
/// Command to turn on a smart light.
/// </summary>
public class LightOnCommand : ICommand
{
    private readonly SmartLight _light;
    private readonly int _brightness;
    private (bool IsOn, int Brightness) _previousState;

    public LightOnCommand(SmartLight light, int brightness = 100)
    {
        _light = light;
        _brightness = brightness;
    }

    public void Execute()
    {
        _previousState = _light.GetState();
        _light.TurnOn(_brightness);
    }

    public void Undo()
    {
        if (_previousState.IsOn)
        {
            _light.TurnOn(_previousState.Brightness);
        }
        else
        {
            _light.TurnOff();
        }
        Console.WriteLine($"    ↶ Undid: Turn on {_light.Location} light");
    }

    public string GetDescription() => $"Turn on {_light.Location} light (brightness: {_brightness}%)";
}

/// <summary>
/// Command to turn off a smart light.
/// </summary>
public class LightOffCommand : ICommand
{
    private readonly SmartLight _light;
    private (bool IsOn, int Brightness) _previousState;

    public LightOffCommand(SmartLight light)
    {
        _light = light;
    }

    public void Execute()
    {
        _previousState = _light.GetState();
        _light.TurnOff();
    }

    public void Undo()
    {
        if (_previousState.IsOn)
        {
            _light.TurnOn(_previousState.Brightness);
        }
        Console.WriteLine($"    ↶ Undid: Turn off {_light.Location} light");
    }

    public string GetDescription() => $"Turn off {_light.Location} light";
}

/// <summary>
/// Command to set thermostat temperature.
/// </summary>
public class SetTemperatureCommand : ICommand
{
    private readonly SmartThermostat _thermostat;
    private readonly int _temperature;
    private int _previousTemperature;

    public SetTemperatureCommand(SmartThermostat thermostat, int temperature)
    {
        _thermostat = thermostat;
        _temperature = temperature;
    }

    public void Execute()
    {
        _previousTemperature = _thermostat.GetTemperature();
        _thermostat.SetTemperature(_temperature);
    }

    public void Undo()
    {
        _thermostat.SetTemperature(_previousTemperature);
        Console.WriteLine($"    ↶ Undid: Set {_thermostat.Zone} thermostat to {_temperature}°F");
    }

    public string GetDescription() => $"Set {_thermostat.Zone} thermostat to {_temperature}°F";
}

/// <summary>
/// Command to play music on the music system.
/// </summary>
public class PlayMusicCommand : ICommand
{
    private readonly MusicSystem _musicSystem;
    private readonly string _playlist;
    private (bool IsPlaying, string? Playlist, int Volume) _previousState;

    public PlayMusicCommand(MusicSystem musicSystem, string playlist)
    {
        _musicSystem = musicSystem;
        _playlist = playlist;
    }

    public void Execute()
    {
        _previousState = _musicSystem.GetState();
        _musicSystem.Play(_playlist);
    }

    public void Undo()
    {
        if (_previousState.IsPlaying && _previousState.Playlist is not null)
        {
            _musicSystem.Play(_previousState.Playlist);
        }
        else
        {
            _musicSystem.Stop();
        }
        Console.WriteLine($"    ↶ Undid: Play {_playlist} on {_musicSystem.Name}");
    }

    public string GetDescription() => $"Play {_playlist} on {_musicSystem.Name}";
}

/// <summary>
/// Command to set music system volume.
/// </summary>
public class SetVolumeCommand : ICommand
{
    private readonly MusicSystem _musicSystem;
    private readonly int _volume;
    private int _previousVolume;

    public SetVolumeCommand(MusicSystem musicSystem, int volume)
    {
        _musicSystem = musicSystem;
        _volume = volume;
    }

    public void Execute()
    {
        _previousVolume = _musicSystem.GetState().Volume;
        _musicSystem.SetVolume(_volume);
    }

    public void Undo()
    {
        _musicSystem.SetVolume(_previousVolume);
        Console.WriteLine($"    ↶ Undid: Set {_musicSystem.Name} volume to {_volume}%");
    }

    public string GetDescription() => $"Set {_musicSystem.Name} volume to {_volume}%";
}

/// <summary>
/// Macro command that executes multiple commands in sequence.
/// </summary>
public class MacroCommand : ICommand
{
    private readonly string _name;
    private readonly List<ICommand> _commands;

    public MacroCommand(string name, List<ICommand> commands)
    {
        _name = name;
        _commands = commands;
    }

    public void Execute()
    {
        Console.WriteLine($"    🎬 Executing macro: {_name}");
        foreach (var command in _commands)
        {
            command.Execute();
        }
    }

    public void Undo()
    {
        Console.WriteLine($"    ↶ Undoing macro: {_name}");
        // Undo in reverse order
        for (int i = _commands.Count - 1; i >= 0; i--)
        {
            _commands[i].Undo();
        }
    }

    public string GetDescription() => $"Macro: {_name} ({_commands.Count} commands)";
}

/// <summary>
/// No-operation command for empty slots.
/// </summary>
public class NoCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("    ⭕ No command assigned");
    }

    public void Undo()
    {
        Console.WriteLine("    ⭕ Nothing to undo");
    }

    public string GetDescription() => "No command";
}

// Invoker classes
/// <summary>
/// Smart home remote control that can execute commands and maintain undo history.
/// </summary>
public class SmartHomeRemote
{
    private ICommand _command = new NoCommand();
    private readonly Stack<ICommand> _undoStack = new();

    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public void PressButton()
    {
        Console.WriteLine($"  🎛️ Executing: {_command.GetDescription()}");
        _command.Execute();
        _undoStack.Push(_command);
    }

    public void PressUndo()
    {
        if (_undoStack.Count > 0)
        {
            var lastCommand = _undoStack.Pop();
            Console.WriteLine($"  🔄 Undoing: {lastCommand.GetDescription()}");
            lastCommand.Undo();
        }
        else
        {
            Console.WriteLine("  ❌ Nothing to undo");
        }
    }

    public void ClearHistory()
    {
        _undoStack.Clear();
        Console.WriteLine("  🗑️ Command history cleared");
    }
}

/// <summary>
/// Command scheduler that can queue and execute commands at specific times.
/// </summary>
public class CommandScheduler
{
    private readonly List<ScheduledCommand> _scheduledCommands = [];

    public void ScheduleCommand(ICommand command, DateTime executeAt, string description = "")
    {
        _scheduledCommands.Add(new ScheduledCommand(command, executeAt, description));
        Console.WriteLine($"    📅 Scheduled: {command.GetDescription()} at {executeAt:HH:mm:ss}");
    }

    public void ExecuteScheduledCommands()
    {
        var commandsToExecute = _scheduledCommands
            .Where(sc => sc.ExecuteAt <= DateTime.Now)
            .OrderBy(sc => sc.ExecuteAt)
            .ToList();

        foreach (var scheduledCommand in commandsToExecute)
        {
            Console.WriteLine($"    ⏰ Executing scheduled: {scheduledCommand.Description}");
            scheduledCommand.Command.Execute();
            _scheduledCommands.Remove(scheduledCommand);
            
            // Small delay to show the scheduling effect
            System.Threading.Thread.Sleep(500);
        }

        if (commandsToExecute.Count == 0)
        {
            Console.WriteLine("    ℹ️ No commands ready for execution");
        }
    }

    private record ScheduledCommand(ICommand Command, DateTime ExecuteAt, string Description);
}