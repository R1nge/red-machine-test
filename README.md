"Level1" - созданный мной уровень  
Старался уложиться в существующую архитектуру, так что почти ничего не менял  
Надеюсь, что PlayerState.Scrolling был предназначен для передвижения камеры  


https://github.com/R1nge/red-machine-test/assets/59400159/a8da7a10-20b1-4954-aa09-584ef6b2c987

Чтобы упростить жизнь проверяющему, распишу всё, что я сделал  
В классе ClickObserver, подписался на ClickHandler.DragStartEvent  
Там же проверяю стейт, если не Connecting, то  
ClickObserver отправляет PlayerFingerPlaced в EventControllers  
PlayerFingerPlaced - мой евент, созданный по аналогу уже созданных  
CameraMovement подписывается на PlayerFingerPlaced и PlayerFingerRemoved  
И обновляет свой внутренний стейт (bool _dragging)  
CameraMovement содержит всю логику передвижения  
