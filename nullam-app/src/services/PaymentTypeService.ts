import {BaseService} from "./BaseService";
import {IPaymentType} from "../domain/IPaymentType";

export class PaymentTypeService extends BaseService<IPaymentType>{
    constructor() {
        super("paymenttypes");
    }
}