import {Dispatch} from "@reduxjs/toolkit";
import api from "../api";
import {
    deleteHistoryItemLocal,
    documentProcessFailure,
    documentProcessStart,
    documentProcessSuccess, myHistoryFailure,
    myHistoryStart,
    myHistorySuccess
} from "./misc.slice.ts";
import {deleteHistoryItem} from "../api/misc";

export const uploadFilesAC = (data: File[]) =>
    async (dispatch: Dispatch) => {
        try {
            dispatch(documentProcessStart());
            const res = await api.misc.uploadFiles(data);
            dispatch(documentProcessSuccess(res.data.id));
        } catch (e: any) {
            console.error(e);
            dispatch(documentProcessFailure(e.message));
        }
    }

export const getHistoryAC = () =>
    async (dispatch: Dispatch) => {
        try {
            dispatch(myHistoryStart());
            const res = await api.misc.getHistory();
            dispatch(myHistorySuccess(res.data));
        } catch (e: any) {
            console.error(e);
            dispatch(myHistoryFailure(e.message));
        }
    }

export const deleteHistoryItemAC = (ids: number[]) =>
    async (dispatch: Dispatch) => {
        try {
            const res = await api.misc.deleteHistoryItem(ids);
            dispatch(deleteHistoryItemLocal(ids));
        } catch (e: any) {
            console.error(e);
        }
    }