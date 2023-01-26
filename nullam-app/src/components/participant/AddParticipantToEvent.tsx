import {Link, useNavigate} from "react-router-dom";
import React, {FormEvent, useEffect, useState} from "react";
import {PersonService} from "../../services/PersonService";
import {EnterpriseService} from "../../services/EnterpriseService";
import {ParticipantService} from "../../services/ParticipantService";
import {IPerson} from "../../domain/IPerson";
import {IParticipant} from "../../domain/IParticipant";
import {IEnterprise} from "../../domain/IEnterprise";
import {PaymentTypeService} from "../../services/PaymentTypeService";
import {IServiceResult} from "../../domain/IServiceResult";
import {IPaymentType} from "../../domain/IPaymentType";

interface IProps{
    eventId: string,
    attachParticipantToEvent: (participant: IParticipant) => void,

    isHeld: boolean
}

let paymentTypeStateInit: IServiceResult<IPaymentType[]>;
const AddParticipantToEvent = (props: IProps) => {

    const personService = new PersonService();
    const enterpriseService = new EnterpriseService();
    const participantService = new ParticipantService();
    const paymentTypeService = new PaymentTypeService();

    const navigate = useNavigate();

    const [paymentTypeState, setPaymentTypeState] = useState(paymentTypeStateInit)
    const [formDataPerson, setFormDataPerson] = useState({
        firstName: '',
        lastName: '',
        identificationNumber: '',
        paymentType: '',
        additionalDetails: '',
        errorMsg: [] as string[]
    });

    const [formDataEnterprise, setFormDataEnterprise] = useState({
        legalName: '',
        registerCode: '',
        participantsNumber: 0,
        paymentType: '',
        additionalDetails: '',
        errorMsg: [] as string[]
    });

    const [checkboxState, setCheckboxState] = useState(0);

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
        if (checkboxState === 0){
            let personByIdentityCode = await personService.getByIdentityCode(formDataPerson.identificationNumber, props.eventId);
            if (personByIdentityCode.status === 200){

                await personService.put({
                    id: personByIdentityCode.data?.id,
                    firstName: formDataPerson.firstName,
                    lastName: formDataPerson.lastName,
                    identificationNumber: formDataPerson.identificationNumber,
                    additionalDetails: formDataPerson.additionalDetails
                }, personByIdentityCode.data?.id as string);

                let participantResult = await participantService.add({
                    eventId: props.eventId,
                    personId: personByIdentityCode.data?.id,
                    paymentTypeId: formDataPerson.paymentType
                });

                setFormDataPerson({
                    firstName: '',
                    lastName: '',
                    identificationNumber: '',
                    paymentType: '',
                    additionalDetails: '',
                    errorMsg: []
                });

                props.attachParticipantToEvent(participantResult.data as IParticipant)
                navigate("/");
            } else {
                const person: IPerson = {
                    firstName: formDataPerson.firstName,
                    lastName: formDataPerson.lastName,
                    identificationNumber: formDataPerson.identificationNumber,
                    additionalDetails: formDataPerson.additionalDetails
                };
                let personResult = await personService.add(person);
                if (personResult.status === 201){
                    const participant: IParticipant = {
                        eventId: props.eventId,
                        personId: personResult.data?.id,
                        paymentTypeId: formDataPerson.paymentType
                    };

                    let participantResult = await participantService.add(participant);

                    setFormDataPerson({
                        firstName: '',
                        lastName: '',
                        identificationNumber: '',
                        paymentType: '',
                        additionalDetails: '',
                        errorMsg: []
                    });

                    props.attachParticipantToEvent(participantResult.data as IParticipant)
                    navigate("/");
                } else {
                    const errors: string[] = personResult.errorMsg!["errorMsg"];
                    setFormDataPerson({ ...formDataPerson, errorMsg: errors});
                }
            }
        }

        if (checkboxState === 1){
            let enterpriseByRegisterCode = await enterpriseService.getByRegisterCode(formDataEnterprise.registerCode, props.eventId);
            if (enterpriseByRegisterCode.status === 200){

                await enterpriseService.put({
                    id: enterpriseByRegisterCode.data?.id,
                    legalName: formDataEnterprise.legalName,
                    registerCode: formDataEnterprise.registerCode,
                    participantsNumber: formDataEnterprise.participantsNumber,
                    additionalDetails: formDataEnterprise.additionalDetails
                }, enterpriseByRegisterCode.data?.id as string)

                let participantResult = await participantService.add({
                    eventId: props.eventId,
                    enterpriseId: enterpriseByRegisterCode.data?.id,
                    paymentTypeId: formDataEnterprise.paymentType
                });

                setFormDataEnterprise({
                    legalName: '',
                    registerCode: '',
                    participantsNumber: 0,
                    paymentType: '',
                    additionalDetails: '',
                    errorMsg: []
                })

                props.attachParticipantToEvent(participantResult.data as IParticipant)
                navigate("/");
            } else {
                const enterprise: IEnterprise = {
                    legalName: formDataEnterprise.legalName,
                    registerCode: formDataEnterprise.registerCode,
                    participantsNumber: formDataEnterprise.participantsNumber,
                    additionalDetails: formDataEnterprise.additionalDetails
                };

                let enterpriseResult = await enterpriseService.add(enterprise);

                if (enterpriseResult.status === 201){
                    const participant: IParticipant = {
                        eventId: props.eventId,
                        enterpriseId: enterpriseResult.data?.id,
                        paymentTypeId: formDataEnterprise.paymentType
                    };
                    let participantResult = await participantService.add(participant);

                    setFormDataEnterprise({
                        legalName: '',
                        registerCode: '',
                        participantsNumber: 0,
                        paymentType: '',
                        additionalDetails: '',
                        errorMsg: []
                    })

                    props.attachParticipantToEvent(participantResult.data as IParticipant)
                    navigate("/");
                } else {
                    const errors: string[] = enterpriseResult.errorMsg!["errorMsg"];
                    setFormDataEnterprise({ ...formDataEnterprise, errorMsg: errors});
                }
            }
        }
    }

    return (
        <div>
            <div className="row align-items-md-stretch">
                <div className="col-md-3"></div>
                <div className="col-md-6">
                    {!props.isHeld ? (
                        <form onSubmit={(e) => handleSubmit(e)}>
                            <h3 style={{color: "#0d6efd"}}>Osavõtjate lisamine</h3>
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
                            <div className="row align-items-md-stretch formRowItem">
                                <div className="col-md-3">
                                </div>
                                <div className="col-md-9">
                                    <div className="row" style={{marginLeft: "0px"}}>
                                        <div className="col-md-6 form-check">
                                            <input checked={checkboxState === 0} onChange={() => setCheckboxState(0)} className="form-check-input" type="radio" name="flexRadioDefault"
                                                   id="flexRadioDefault1"/>
                                            <label className="form-check-label" htmlFor="flexRadioDefault1">
                                                Eraisik
                                            </label>
                                        </div>
                                        <div className="col-md-6 form-check">
                                            <input onChange={() => setCheckboxState(1)} className="form-check-input" type="radio" name="flexRadioDefault"
                                                   id="flexRadioDefault1"/>
                                            <label className="form-check-label" htmlFor="flexRadioDefault1">
                                                Ettevõtte
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            {checkboxState === 0 ? (
                                <div>
                                    <div className="row align-items-md-stretch formRowItem">
                                        <div className="col-md-3">
                                            <label htmlFor="name" className="col-form-label">Eesnimi:</label>
                                        </div>
                                        <div className="col-md-9">
                                            <input
                                                required
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
                                                value={formDataPerson.paymentType}
                                                onChange={(e) => handleChangePerson(e.target)}
                                                className="form-select"
                                                aria-label="Valige makseviis"
                                                name="paymentType"
                                                id="paymentType"
                                            >
                                                <option disabled={true}></option>
                                                {paymentTypeState.data?.map((item, index) => (
                                                    <option
                                                        key={index}
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
                                                value={formDataEnterprise.paymentType}
                                                onChange={(e) => handleChangeEnterprise(e.target)}
                                                className="form-select"
                                                aria-label="Valige makseviis"
                                                name="paymentType"
                                                id="paymentType"
                                            >
                                                <option disabled={true}></option>
                                                {paymentTypeState.data?.map((item, index) => (
                                                    <option
                                                        key={index}
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
                            <Link to="/" type="button" className="btn btn-secondary buttonsMargin" style={{marginLeft: 0}}>Tagasi</Link>
                            {/*<button type="button" className="btn btn-primary buttonsMargin">Lisa</button>*/}
                            <input type="submit" value="Salvesta" className="btn btn-primary buttonsMargin"/>
                        </form>
                    ) : (
                        <h3>Üritus on juba lõppenud</h3>
                    )
                    }
                </div>
                <div className="col-md-3"></div>
            </div>
        </div>
    )
}

export default AddParticipantToEvent;