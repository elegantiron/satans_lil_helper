if exist .venv/Scripts/activate.bat (
    CALL .venv/Scripts/activate.bat
) else (
    python.exe -m venv .venv
    CALL .venv/Scripts/activate.bat
    python -m pip install -U pip
)
pip install -U -r requirements.txt
pyinstaller -y -F -w -n "Satans Lil Helper" --add-data assets:assets main.py