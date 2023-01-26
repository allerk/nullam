import {BaseService} from "./BaseService";
import {IPerson} from "../domain/IPerson";
import {IServiceResult} from "../domain/IServiceResult";
import httpClient from "./HttpClient";
import {AxiosError} from "axios";

export class PersonService extends BaseService<IPerson>{
    constructor() {
        super("persons");
    }

    async getByIdentityCode(identityCode: string, eventId: string): Promise<IServiceResult<IPerson>> {
        try {
            let response = await httpClient.get(`/v1.0/persons/identityCode/${identityCode}/${eventId}`);
            return {
                status: response.status,
                data: response.data as IPerson
            };
        } catch (e) {
            return {
                status: (e as AxiosError<any, any>).response!.status,
                errorMsg: (e as AxiosError<any, any>).response!.data.errors
            }
        }
    }
}