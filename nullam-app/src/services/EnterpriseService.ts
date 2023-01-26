import {BaseService} from "./BaseService";
import {IEnterprise} from "../domain/IEnterprise";
import {IServiceResult} from "../domain/IServiceResult";
import {IPerson} from "../domain/IPerson";
import httpClient from "./HttpClient";
import {AxiosError} from "axios";

export class EnterpriseService extends BaseService<IEnterprise>{
    constructor() {
        super("enterprises");
    }

    async getByRegisterCode(registerCode: string, eventId: string): Promise<IServiceResult<IEnterprise>> {
        try {
            let response = await httpClient.get(`/v1.0/enterprises/registerCode/${registerCode}/${eventId}`);
            return {
                status: response.status,
                data: response.data as IEnterprise
            };
        } catch (e) {
            return {
                status: (e as AxiosError<any, any>).response!.status,
                errorMsg: (e as AxiosError<any, any>).response!.data.errors
            }
        }
    }
}