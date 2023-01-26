import {IError} from "./IError";

export interface IServiceResult<TData> {
    status: number;
    data?: TData;
    errorMsg?: IError;
}

