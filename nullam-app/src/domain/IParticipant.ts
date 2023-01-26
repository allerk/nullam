import {IPerson} from "./IPerson";
import {IEnterprise} from "./IEnterprise";
import {IPaymentType} from "./IPaymentType";

export interface IParticipant {
    id?: string,
    paymentTypeId: string,
    paymentType?: IPaymentType,
    personId?: string,
    person?: IPerson,
    enterpriseId?: string,
    enterprise?: IEnterprise,
    eventId: string
}