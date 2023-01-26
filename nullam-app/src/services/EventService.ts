import {BaseService} from "./BaseService";
import {IEvent} from "../domain/IEvent";

export class EventService extends BaseService<IEvent>{
    constructor() {
        super("events");
    }
}