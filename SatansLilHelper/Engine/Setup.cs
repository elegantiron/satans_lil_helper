using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SatansLilHelper.ECSComponents;
using SatansLilHelper.GameComponents;
using SatansLilHelper.Types;

namespace SatansLilHelper;

internal partial class Engine
{
    private GameScreen _gameScreen;
    private TitleScreen _titleScreen;
    private MainMenu _mainMenuScreen;
    private SatanFace _satanScreen;
    private StatusWindow _statusScreen;
    private PlayerTurn _playerTurn;
    private PauseMenu _pauseMenu;
    private Minimap _miniMap;
    private Megamap _megaMap;
    private IncomingLinks<Inventory> _inventory;
    private IncomingLinks<Grimoire> _grimoire;
    private ConfirmPopup _confirmPopup;

    public GameScreen GameScreen => _gameScreen;
    public TitleScreen TitleScreen => _titleScreen;
    public MainMenu MainMenuScreen => _mainMenuScreen;
    public SatanFace SatanScreen => _satanScreen;
    public StatusWindow StatusScreen => _statusScreen;
    public PlayerTurn PlayerTurn => _playerTurn;
    public PauseMenu PauseMenu => _pauseMenu;
    public Minimap MiniMap => _miniMap;
    public Megamap MegaMap => _megaMap;
    public IncomingLinks<Inventory> Inventory => _inventory;
    public IncomingLinks<Grimoire> Grimoire => _grimoire;
    public ConfirmPopup ConfirmPopup => _confirmPopup;

    [MemberNotNull(
        nameof(_titleScreen),
        nameof(_mainMenuScreen),
        nameof(_satanScreen),
        nameof(_gameScreen),
        nameof(_statusScreen),
        nameof(_pauseMenu),
        nameof(_miniMap),
        nameof(_inventory),
        nameof(_megaMap),
        nameof(_grimoire),
        nameof(_confirmPopup)
    )]
    private void SetupGameComponents()
    {
        _titleScreen = new(this) { Visible = false, Enabled = false };
        _mainMenuScreen = new(this) { Visible = false, Enabled = false };
        _satanScreen = new(this) { Visible = false, Enabled = false };
        _gameScreen = new(this) { Visible = false, Enabled = false };
        _statusScreen = new(this) { Visible = false, Enabled = false };
        _pauseMenu = new(this) { Visible = false, Enabled = false };
        _miniMap = new(this) { Visible = false, Enabled = false };
        _megaMap = new(this) { Visible = false, Enabled = false };
        _inventory = new(this) { Visible = false, Enabled = false };
        _grimoire = new(this) { Visible = false, Enabled = false };
        _confirmPopup = new(this) { Visible = false, Enabled = false };

        Components.Add(_titleScreen);
        Components.Add(_mainMenuScreen);
        Components.Add(_satanScreen);
        Components.Add(_gameScreen);
        Components.Add(_statusScreen);
        Components.Add(_pauseMenu);
        Components.Add(_miniMap);
        Components.Add(_megaMap);
        Components.Add(_inventory);
        Components.Add(_grimoire);
        Components.Add(_confirmPopup);
    }
}
