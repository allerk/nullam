import BannerSmall from "../BannerSmall";
import React, {FormEvent, useEffect, useState} from "react";
import {Link, useLocation, useNavigate, useParams} from "react-router-dom";
import {IParticipant} from "../../domain/IParticipant";
import {PaymentTypeService} from "../../services/PaymentTypeService";
import {IServiceResult} from "../../domain/IServiceResult";
import {IPaymentType} from "../../domain/IPaymentType";
import {ParticipantService} from "../../services/ParticipantService";
import {PersonService} from "../../services/PersonService";
import {EnterpriseService} from "../../services/EnterpriseService";
import {IPerson} from "../../domain/IPerson";
import {IEnterprise} from "../../domain/IEnterprise";
import {EventService} from "../../services/EventService";


let paymentTypeStateInit: IServiceResult<IPaymentType[]>;
const ParticipantDetails = () => {

    const participantService = new ParticipantService();
    const eventService = new EventService();
    const personService = new PersonService();
    const enterpriseService = new EnterpriseService();
    const paymentTypeService = new PaymentTypeService();

    const [paymentTypeState, setPaymentTypeState] = useState(paymentTypeStateInit);

    const location = useLocation();
    const navigate = useNavigate();
    const { participantType } = location.state;
    const { eventId } = location.state;
    const { registerCode } = location.state;
    const { identificationNumber } = location.state;
    const { isHeld } = location.state;

    const [formDataPerson, setFormDataPerson] = useState({
        id: '',
        firstName: '',
        lastName: '',
        identificationNumber: '',
        additionalDetails: '',
        paymentType: '',
        participantId: '',
        errorMsg: [] as string[]
    });

    const [formDataEnterprise, setFormDataEnterprise] = useState({
        id: '',
        legalName: '',
        registerCode: '',
        participantsNumber: 0,
        additionalDetails: '',
        paymentType: '',
        participantId: '',
        errorMsg: [] as string[]
    });

    const { participantId } = useParams();

    useEffect(() => {
        if (participantType === "0"){
            participantService.getById(participantId as string).then(data => setFormDataPerson({
                id: data.data?.personId as string,
                firstName: data.data?.person?.firstName as string,
                lastName: data.data?.person?.lastName as string,
                identificationNumber: data.data?.person?.identificationNumber as string,
                additionalDetails: data.data?.person?.additionalDetails as string,
                paymentType: data.data?.paymentTypeId as string,
                participantId: data.data?.id as string,
                errorMsg: []
            }));
        }
        if (participantType === "1"){
            participantService.getById(participantId as string).then(data => setFormDataEnterprise({
                id: data.data?.enterpriseId as string,
                legalName: data.data?.enterprise?.legalName as string,
                registerCode: data.data?.enterprise?.registerCode as string,
                participantsNumber: data.data?.enterprise?.participantsNumber as number,
                additionalDetails: data.data?.enterprise?.additionalDetails as string,
                paymentType: data.data?.paymentTypeId as string,
                participantId: data.data?.id as string,
                errorMsg: []
            }));
        }
    }, []);

    useEffect(() => {
        paymentTypeService.getAll().then(data => setPaymentTypeState(data));
    }, []);

    if (!paymentTypeState){
        return null;
    }

    const handleChangePerson = (event:
                                    EventTarget & HTMLInputElement |
                                    EventTarget & HTMLSelectElement |
                                    EventTarget & HTMLTextAreaElement) => {
        setFormDataPerson({ ...formDataPerson, [event.name]: event.value });
    }

    const handleChangeEnterprise = (event:
                                        EventTarget & HTMLInputElement |
                                        EventTarget & HTMLSelectElement |
                                        EventTarget & HTMLTextAreaElement) => {
        setFormDataEnterprise({ ...formDataEnterprise, [event.name]: event.value });
    }

    const handleSubmit = async (event: FormEvent) => {
        event.preventDefault();
        if (participantType === "0"){
            if (formDataPerson.identificationNumber !== identificationNumber) {
                const events = await eventService.getAll();
                let doesAnyoneElseHave = false;
                let errorMsg: string[] = [];
                for (const elem of events.data!) {
                    if (elem.id === eventId) {
                        continue;
                    }
                    const res = await personService.getByIdentityCode(formDataPerson.identificationNumber, elem.id as string);
                    if (res.status === 400){
                        doesAnyoneElseHave = true;
                        errorMsg = res.errorMsg!["errorMsg"];
                    }
                }
                let personCheck = await personService.getByIdentityCode(formDataPerson.identificationNumber, eventId);
                if (personCheck.status !== 400 && !doesAnyoneElseHave){
                    const person: IPerson = {
                        id: formDataPerson.id,
                        firstName: formDataPerson.firstName,
                        lastName: formDataPerson.lastName,
                        identificationNumber: formDataPerson.identificationNumber,
                        additionalDetails: formDataPerson.additionalDetails
                    };
                    let personResult = await personService.put(person, formDataPerson.id);

                    if (personResult.status === 204) {
                        const participant: IParticipant = {
                            id: formDataPerson.participantId,
                            eventId: eventId,
                            personId: personResult.data?.id,
                            person: person,
                            paymentTypeId: formDataPerson.paymentType
                        };
                        let participantResult = await participantService.put(participant, formDataPerson.participantId);

                        setFormDataPerson({
                            id: '',
                            firstName: '',
                            lastName: '',
                            identificationNumber: '',
                            additionalDetails: '',
                            paymentType: '',
                            participantId: '',
                            errorMsg: []
                        });
                        navigate(`/event/${eventId}`);
                    } else {
                        const errors: string[] = personResult.errorMsg!["errorMsg"];
                        setFormDataPerson({ ...formDataPerson, errorMsg: errors});
                    }
                } else {
                    if (errorMsg.length === 0){
                        setFormDataPerson({ ...formDataPerson, errorMsg: personCheck.errorMsg!["errorMsg"]});
                    } else {
                        setFormDataPerson({ ...formDataPerson, errorMsg: errorMsg});
                    }
                }
            } else {
                const person: IPerson = {
                    id: formDataPerson.id,
                    firstName: formDataPerson.firstName,
                    lastName: formDataPerson.lastName,
                    identificationNumber: formDataPerson.identificationNumber,
                    additionalDetails: formDataPerson.additionalDetails
                };

                let personResult2 = await personService.put(person, formDataPerson.id);

                if (personResult2.status === 204) {
                    const participant: IParticipant = {
                        id: formDataPerson.participantId,
                        eventId: eventId,
                        personId: personResult2.data?.id,
                        person: person,
                        paymentTypeId: formDataPerson.paymentType
                    };
                    let participantResult = await participantService.put(participant, formDataPerson.participantId);

                    setFormDataPerson({
                        id: '',
                        firstName: '',
                        lastName: '',
                        identificationNumber: '',
                        additionalDetails: '',
                        paymentType: '',
                        participantId: '',
                        errorMsg: []
                    });
                    navigate(`/event/${eventId}`);
                } else {
                    const errors: string[] = personResult2.errorMsg!["errorMsg"];
                    setFormDataPerson({ ...formDataPerson, errorMsg: errors});
                }
            }

        }
        if (participantType === "1"){
            if (registerCode !== formDataEnterprise.registerCode) {
                const eventsEnterprise = await eventService.getAll();
                let doesAnyoneElseHaveEnterprise = false;
                let errorMsg: string[] = [];
                for (const elem of eventsEnterprise.data!) {
                    if (elem.id === eventId) {
                        continue;
                    }
                    const res = await enterpriseService.getByRegisterCode(formDataEnterprise.registerCode, elem.id as string);
                    if (res.status === 400){
                        doesAnyoneElseHaveEnterprise = true;
                        errorMsg = res.errorMsg!["errorMsg"];
                    }
                }
                let enterpriseCheck = await enterpriseService.getByRegisterCode(formDataEnterprise.registerCode, eventId);
                if (enterpriseCheck.status !== 400 && !doesAnyoneElseHaveEnterprise) {
                    const enterprise: IEnterprise = {
                        id: formDataEnterprise.id,
                        legalName: formDataEnterprise.legalName,
                        registerCode: formDataEnterprise.registerCode,
                        participantsNumber: formDataEnterprise.participantsNumber,
                        additionalDetails: formDataEnterprise.additionalDetails
                    };
                    let enterpriseResult = await enterpriseService.put(enterprise, formDataEnterprise.id);

                    const participant: IParticipant = {
                        id: formDataEnterprise.participantId,
                        eventId: eventId,
                        enterpriseId: enterpriseResult.data?.id,
                        enterprise: enterprise,
                        paymentTypeId: formDataEnterprise.paymentType
                    };
                    let participantResult = await participantService.put(participant, formDataEnterprise.participantId);

                    setFormDataEnterprise({
                        id: '',
                        legalName: '',
                        registerCode: '',
                        participantsNumber: 0,
                        paymentType: '',
                        additionalDetails: '',
                        participantId: '',
                        errorMsg: []
                    })
                    navigate(`/event/${eventId}`);
                } else {
                    if (errorMsg.length === 0){
                        setFormDataEnterprise({ ...formDataEnterprise, errorMsg: enterpriseCheck.errorMsg!["errorMsg"]});
                    } else {
                        setFormDataEnterprise({ ...formDataEnterprise, errorMsg: errorMsg});
                    }
                }
            } else {
                const enterprise: IEnterprise = {
                    id: formDataEnterprise.id,
                    legalName: formDataEnterprise.legalName,
                    registerCode: formDataEnterprise.registerCode,
                    participantsNumber: formDataEnterprise.participantsNumber,
                    additionalDetails: formDataEnterprise.additionalDetails
                };
                let enterpriseResult = await enterpriseService.put(enterprise, formDataEnterprise.id);

                const participant: IParticipant = {
                    id: formDataEnterprise.participantId,
                    eventId: eventId,
                    enterpriseId: enterpriseResult.data?.id,
                    enterprise: enterprise,
                    paymentTypeId: formDataEnterprise.paymentType
                };
                let participantResult = await participantService.put(participant, formDataEnterprise.participantId);

                setFormDataEnterprise({
                    id: '',
                    legalName: '',
                    registerCode: '',
                    participantsNumber: 0,
                    paymentType: '',
                    additionalDetails: '',
                    participantId: '',
                    errorMsg: []
                })
                navigate(`/event/${eventId}`);
            }
        }
    }

    return (
        <div className="container-md">
            <BannerSmall pageName={"Osavõtjad"}></BannerSmall>
            <div className="container-md" style={{backgroundColor: "white"}}>
                <div className="row align-items-md-stretch">
                    <div className="col-md-3"></div>
                    <div className="col-md-6">
                        <form onSubmit={(e) => handleSubmit(e)}>
                            <h3 style={{color: "#0d6efd"}}>Osavõtja info</h3>
                            <div className="text-danger validation-summary-errors">
                                {formDataPerson.errorMsg.length > 0 &&
                                    <div>
                                        {formDataPerson.errorMsg.map(error => (
                                            <ul>
                                                <li>{error}</li>
                                            </ul>
                                        ))}
                                    </div>
                                }
                                {formDataEnterprise.errorMsg.length > 0 &&
                                    <div>
                                        {formDataEnterprise.errorMsg.map(error => (
                                            <ul>
                                                <li>{error}</li>
                                            </ul>
                                        ))}
                                    </div>
                                }
                            </div>
                            {participantType === "0" ? (
                                <div>
                                    <div className="row align-items-md-stretch formRowItem">
                                        <div className="col-md-3">
                                            <label htmlFor="name" className="col-form-label">Eesnimi:</label>
                                        </div>
                                        <div className="col-md-9">
                                            <input
                                                required
                                                disabled={isHeld === "true"}
                                                type="text"
                                                name="firstName"
                                                value={formDataPerson.firstName}
                                                onChange={(e) => handleChangePerson(e.target)}
                                                className="form-control"
                                            />
                                        </div>
                                    </div>
                                    <div className="row align-items-md-stretch formRowItem">
                                        <div className="col-md-3">
                                            <label htmlFor="eventTime" className="col-form-label">Perenimi:</label>
                                        </div>
                                        <div className="col-md-9">
                                            <input
                                                required
                                                disabled={isHeld === "true"}
                                                type="text"
                                                name="lastName"
                                                value={formDataPerson.lastName}
                                                onChange={(e) => handleChangePerson(e.target)}
                                                className="form-control"
                                            />
                                        </div>
                                    </div>
                                    <div className="row align-items-md-stretch formRowItem">
                                        <div className="col-md-3">
                                            <label htmlFor="location" className="col-form-label">Isikukood:</label>
                                        </div>
                                        <div className="col-md-9">
                                            <input
                                                required
                                                disabled={isHeld === "true"}
                                                type="text"
                                                name="identificationNumber"
                                                value={formDataPerson.identificationNumber}
                                                onChange={(e) => handleChangePerson(e.target)}
                                                className="form-control"
                                            />
                                        </div>
                                    </div>
                                    <div className="row align-items-md-stretch formRowItem">
                                        <div className="col-md-3">
                                            <label htmlFor="location" className="col-form-label">Maksmisviis:</label>
                                        </div>
                                        <div className="col-md-9">
                                            <select
                                                disabled={isHeld === "true"}
                                                value={formDataPerson.paymentType}
                                                onChange={(e) => handleChangePerson(e.target)}
                                                className="form-select"
                                                aria-label="Valige makseviis"
                                                name="paymentType"
                                                id="paymentType"
                                            >
                                                {paymentTypeState.data?.map(item => (
                                                    <option
                                                        key={item.id}
                                                        value={item.id}
                                                    >
                                                        {item.name}
                                                    </option>
                                                ))}
                                            </select>
                                        </div>
                                    </div>
                                    <div className="row align-items-md-stretch formRowItem">
                                        <div className="col-md-3">
                                            <label htmlFor="additionalDetails" className="col-form-label">Lisainfo:</label>
                                        </div>
                                        <div className="col-md-9">
                                <textarea
                                    name="additionalDetails"
                                    disabled={isHeld === "true"}
                                    value={formDataPerson.additionalDetails}
                                    onChange={(e) => handleChangePerson(e.target)}
                                    className="form-control"
                                    style={{height: "100px"}}
                                    maxLength={1000}
                                ></textarea>
                                        </div>
                                    </div>
                                </div>
                            ) : (
                                <div>
                                    <div className="row align-items-md-stretch formRowItem">
                                        <div className="col-md-3">
                                            <label htmlFor="name" className="col-form-label">Juriidiline nimi:</label>
                                        </div>
                                        <div className="col-md-9">
                                            <input
                                                required
                                                disabled={isHeld === "true"}
                                                type="text"
                                                name="legalName"
                                                value={formDataEnterprise.legalName}
                                                onChange={(e) => handleChangeEnterprise(e.target)}
                                                className="form-control"
                                            />
                                        </div>
                                    </div>
                                    <div className="row align-items-md-stretch formRowItem">
                                        <div className="col-md-3">
                                            <label htmlFor="eventTime" className="col-form-label">Registrikood:</label>
                                        </div>
                                        <div className="col-md-9">
                                            <input
                                                required
                                                disabled={isHeld === "true"}
                                                type="text"
                                                name="registerCode"
                                                value={formDataEnterprise.registerCode}
                                                onChange={(e) => handleChangeEnterprise(e.target)}
                                                className="form-control"
                                            />
                                        </div>
                                    </div>
                                    <div className="row align-items-md-stretch formRowItem">
                                        <div className="col-md-3">
                                            <label htmlFor="location" className="col-form-label">Tulevate osavõtjate arv:</label>
                                        </div>
                                        <div className="col-md-9">
                                            <input
                                                required
                                                disabled={isHeld === "true"}
                                                type="number"
                                                min={0}
                                                name="participantsNumber"
                                                value={formDataEnterprise.participantsNumber}
                                                onChange={(e) => handleChangeEnterprise(e.target)}
                                                className="form-control"
                                            />
                                        </div>
                                    </div>
                                    <div className="row align-items-md-stretch formRowItem">
                                        <div className="col-md-3">
                                            <label htmlFor="location" className="col-form-label">Maksmisviis:</label>
                                        </div>
                                        <div className="col-md-9">
                                            <select
                                                disabled={isHeld === "true"}
                                                value={formDataEnterprise.paymentType}
                                                onChange={(e) => handleChangeEnterprise(e.target)}
                                                className="form-select"
                                                aria-label="Valige makseviis"
                                                name="paymentType"
                                                id="paymentType"
                                            >
                                                {paymentTypeState.data?.map(item => (
                                                    <option
                                                        key={item.id}
                                                        value={item.id}
                                                    >
                                                        {item.name}
                                                    </option>
                                                ))}
                                            </select>
                                        </div>
                                    </div>
                                    <div className="row align-items-md-stretch formRowItem">
                                        <div className="col-md-3">
                                            <label htmlFor="additionalDetails" className="col-form-label">Lisainfo:</label>
                                        </div>
                                        <div className="col-md-9">
                                <textarea
                                    disabled={isHeld === "true"}
                                    name="additionalDetails"
                                    value={formDataEnterprise.additionalDetails}
                                    onChange={(e) => handleChangeEnterprise(e.target)}
                                    className="form-control"
                                    style={{height: "100px"}}
                                    maxLength={5000}
                                ></textarea>
                                        </div>
                                    </div>
                                </div>
                            )}
                            <Link to={`/event/${eventId}`} type="button" className="btn btn-secondary buttonsMargin" style={{marginLeft: 0}}>Tagasi</Link>
                            {/*<button type="button" className="btn btn-primary buttonsMargin">Lisa</button>*/}
                            {!isHeld &&
                                <input type="submit" value="Salvesta" className="btn btn-primary buttonsMargin"/>
                            }
                        </form>
                    </div>
                    <div className="col-md-3"></div>
                </div>
            </div>
        </div>
    )
}

export default ParticipantDetails;