import {createSlice, PayloadAction} from "@reduxjs/toolkit";

const initialState = {
    reestrFileList: [] as File[],
    photosFileList: [] as File[],
}

export const mainSlice = createSlice({
    name: 'main',
    initialState,
    reducers: {
        setReestrFileList(state, action: PayloadAction<FileList>) {
            state.reestrFileList = [];
            for (let i = 0; i < action.payload.length; i++) {
                state.reestrFileList.push(action.payload[i]);
            }
        },

        setPhotosFileList(state, action: PayloadAction<FileList>) {
            state.photosFileList = [];
            for (let i = 0; i < action.payload.length; i++) {
                state.photosFileList.push(action.payload[i]);
            }
        },

        deletePhoto(state, action: PayloadAction<File>) {
            if (state.photosFileList.includes(action.payload)) {
                state.photosFileList.splice(state.photosFileList.indexOf(action.payload), 1);
            }

        },

        deleteReestr(state, action: PayloadAction<File>) {
            if (state.reestrFileList.includes(action.payload)) {
                state.reestrFileList.splice(state.reestrFileList.indexOf(action.payload), 1);
            }
        },

        deleteAll(state, action: PayloadAction<File[]>) {
            if (action.payload.toString() === state.reestrFileList.toString()) {
                state.reestrFileList = [];
            }
            else state.photosFileList = [];
        },

        resetMain: () => initialState
    }
})

export const {
    setReestrFileList, setPhotosFileList, resetMain, deletePhoto, deleteReestr, deleteAll
} = mainSlice.actions;

export default mainSlice.reducer;