export interface IHistoryItem {
    "id": number,
    "userId": number,
    "createdAt": string,
    "status": string,
    "errorMessage": string | null,
    "resultArchiveFileName": string
}

export type IHistoryResponse = IHistoryItem[];