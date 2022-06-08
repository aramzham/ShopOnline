function MakeUpdateQtyButtonVisible(id, visible){
    const updateQtyButton = document.querySelector(`button[data-item-id='${id}']`);

    updateQtyButton.style.display = visible === true ? "inline-block" : "none";
}