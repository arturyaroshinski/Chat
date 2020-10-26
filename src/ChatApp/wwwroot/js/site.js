const CREATE_ROOM_BTN = $(".create-room-btn");
const MODAL = $(".modal");
const MODAL_CLOSE_BTN = $(".close");

// Event listeners
CREATE_ROOM_BTN.on("click", () => MODAL.addClass("active"));
MODAL_CLOSE_BTN.on("click", () => MODAL.removeClass("active"));
