import {IParticipant} from "./IParticipant";

export interface IEvent{
    id?: string,
    name: string,
    eventTime: string,
    location: string,
    isHeld: boolean
    additionalDetails: string,
    participants?: IParticipant[]
}