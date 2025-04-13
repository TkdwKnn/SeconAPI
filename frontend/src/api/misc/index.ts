import {AxiosPromise} from "axios";
import {axiosInstance} from "../instance.ts";
import endpoints from "../endpoints.ts";
import {IHistoryResponse} from "./types.ts";

export const uploadFiles = (params: File[]): AxiosPromise<string> =>
    axiosInstance.post(endpoints.documents.process, params, {
        headers: {
            "Content-Type": "multipart/form-data"
        }
    });

export const getHistory = (): AxiosPromise<IHistoryResponse> =>
    axiosInstance.get(endpoints.documents.my);

export const deleteHistoryItem = (ids: number[]): AxiosPromise<string> =>
    axiosInstance.delete(endpoints.documents.deleteHistoryItem(ids));


