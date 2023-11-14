import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { Movie } from "@/types/movie";
import { TvShow } from "@/types/tv-show";

const MODAL_SLICE_NAME = "modal";

type Item = (Movie & TvShow) | null;

interface ModalState {
  isOpen: boolean;
  item: Item;
}

const initialState: ModalState = {
  isOpen: false,
  item: null,
};

const modalSlice = createSlice({
  name: MODAL_SLICE_NAME,
  initialState,
  reducers: {
    openModal: (state, action: PayloadAction<Item>) => {
      state.isOpen = true;
      state.item = action.payload;
    },
    closeModal: (state) => {
      state.isOpen = false;
      state.item = null;
    },
  },
});

export const { openModal, closeModal } = modalSlice.actions;
export default modalSlice.reducer;
