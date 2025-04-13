import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {IHistoryItem} from "../api/misc/types.ts";

const initialState = {
    documentProcessData: {
        isLoading: false,
        taskId: "",
        error: null as string | null,
    },
    myHistoryData: {
        isLoading: false,
        history: null as null | IHistoryItem[],
        error: null as string | null,
    }
}

export const miscSlice = createSlice({
    name: 'misc',
    initialState,
    reducers: {
        myHistoryStart: (state) => {
            state.myHistoryData.isLoading = true;
        },
        myHistorySuccess: (state, action: PayloadAction<IHistoryItem[]>) => {
            state.myHistoryData.isLoading = false;
            state.myHistoryData.history = action.payload;
            state.myHistoryData.error = null;
        },
        myHistoryFailure: (state, action: PayloadAction<string>) => {
            state.myHistoryData.isLoading = false;
            state.myHistoryData.error = action.payload;
        },

        documentProcessStart: (state) => {
            state.documentProcessData.isLoading = true;
        },
        documentProcessSuccess: (state, action: PayloadAction<string>) => {
            state.documentProcessData.isLoading = false;
            state.documentProcessData.taskId = action.payload;
            state.documentProcessData.error = null;
        },
        documentProcessFailure: (state, action: PayloadAction<string>) => {
            state.documentProcessData.isLoading = false;
            state.documentProcessData.error = action.payload;
        },

        deleteHistoryItemLocal: (state, action: PayloadAction<number[]>) => {
            if (state.myHistoryData.history) {
                for (let i = 0; i < state.myHistoryData.history.length; i++) {
                    for (let j = 0; j < action.payload.length; j++) {
                        if (action.payload[j] === state.myHistoryData.history[i].id) {
                            state.myHistoryData.history.splice(i, 1);
                        }
                    }
                }
            }
        },

        resetMisc: () => initialState
    }
})

export const {
    resetMisc, documentProcessSuccess, documentProcessFailure, documentProcessStart,
    myHistorySuccess, myHistoryStart, myHistoryFailure, deleteHistoryItemLocal
} = miscSlice.actions;

export default miscSlice.reducer;