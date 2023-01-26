import {AxiosError} from "axios";
import httpClient from "./HttpClient";
import {IServiceResult} from "../domain/IServiceResult";
import {IError} from "../domain/IError";

export class BaseService<TEntity> {
    constructor(private path: string) {
    }

    async getAll(): Promise<IServiceResult<TEntity[]>> {
        try {
            let response = await httpClient.get(`/v1.0/${this.path}`);
            return {
                status: response.status,
                data: response.data as TEntity[]
            };
        } catch (e) {
            return {
                status: (e as AxiosError<any, any>).response!.status,
                errorMsg: (e as AxiosError<any, any>).response!.data.error
            }
        }
    }

    async getById(id: string): Promise<IServiceResult<TEntity>> {
        try {
            let response = await httpClient.get(`/v1.0/${this.path}/${id}`);
            return {
                status: response.status,
                data: response.data as TEntity
            };
        } catch (e) {
            return {
                status: (e as AxiosError<any, any>).response!.status,
                errorMsg: (e as AxiosError<any, any>).response!.data.error
            }
        }
    }

    
    async add(entity: TEntity): Promise<IServiceResult<TEntity>> {
        let response;
        try {
            response = await httpClient.post(`/v1.0/${this.path}/`, entity);
            return {
                status: response.status,
                data: response.data
            }
        } catch (e) {
            return {
                status: (e as AxiosError<any, any>).response!.status,
                errorMsg: (e as AxiosError<any, any>).response!.data.errors,
            };
        }
    }

    async put(entity: TEntity, id: string): Promise<IServiceResult<TEntity>> {
        let response;
        try {
            response = await httpClient.put(`/v1.0/${this.path}/${id}`, entity);
            return {
                status: response.status,
                data: response.data
            }
        } catch (e) {
            return {
                status: (e as AxiosError<any, any>).response!.status,
                errorMsg: (e as AxiosError<any, any>).response!.data.errors,
            };
        }
    }

    async delete(id: string): Promise<IServiceResult<TEntity>> {
        let response;
        try {
            response = await httpClient.delete(`/v1.0/${this.path}/${id}`);
            return {
                status: response.status,
                data: response.data
            }
        } catch (e) {
            return {
                status: (e as AxiosError<any, any>).response!.status,
                errorMsg: (e as AxiosError<any, any>).response!.data.error,
            };
        }
    }

}
