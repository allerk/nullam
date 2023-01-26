import {BaseService} from "./BaseService";
import {IParticipant} from "../domain/IParticipant";

export class ParticipantService extends BaseService<IParticipant>{
    constructor() {
        super("participants");
    }
}