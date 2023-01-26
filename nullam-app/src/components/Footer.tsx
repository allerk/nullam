import React from 'react';
import "./Footer.css";
import {Link} from "react-router-dom";

const Footer = () =>
    <div className="container">
        <footer className="border-top footer text-muted">
            <div className="container">
                <div className="row footerColor text-white">
                    <div className="col-md-3">
                        <div className="container infoBlock">
                            <h2>
                                Curabitur
                            </h2>
                            <div className="row">
                                <div className="col-md">
                                    <div className="eventItem">Emauris</div>
                                    <div className="eventItem">Kfringilla</div>
                                    <div className="eventItem">Oin magna sem</div>
                                    <div className="eventItem">Kelementum</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="container infoBlock">
                            <h2>
                                Fusce
                            </h2>
                            <div className="row">
                                <div className="col-md">
                                    <div className="eventItem">Econsectetur</div>
                                    <div className="eventItem">Ksollicitudin</div>
                                    <div className="eventItem">Omvulputate</div>
                                    <div className="eventItem">Nunc fringilla tellu</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-6">
                        <div className="container infoBlock">
                            <h2>
                                Kontakt
                            </h2>
                            <div className="row">
                                <div className="col-md-6">
                                    <div className="eventItem office">Peakontor: Tallinnas</div>
                                    <div className="eventItem">Väike-Ameerika 1, 11415 Tallinn</div>
                                    <div className="eventItem">Telefon: 605 4450</div>
                                    <div className="eventItem">Faks: 605 3186</div>
                                </div>
                                <div className="col-md-6">
                                    <div className="eventItem office">Harukontor: Võrus</div>
                                    <div className="eventItem">Oja tn 7 (külastusaadress)</div>
                                    <div className="eventItem">Telefon: 605 3330</div>
                                    <div className="eventItem">Faks: 605 3155</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </footer>
    </div>

export default Footer;