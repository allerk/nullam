import React from 'react';
import { Link } from 'react-router-dom';

import logo from '../images/logo.svg';
import symbol from '../images/symbol.svg';

import "./Header.css";

const Header = () => {

    return (
        // <div className="container">
            <header>
                <div className="container">
                    <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white box-shadow m3">
                        <div className="container container-fluid">
                            <Link className="navbar-brand" to="/"><img src={logo.toString()} alt="React Logo" /></Link>
                            <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                                    aria-expanded="false" aria-label="Toggle navigation">
                                <span className="navbar-toggler-icon"></span>
                            </button>
                            <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between" id="navbarNav">
                                <ul className="navbar-nav">
                                    <li className="nav-item">
                                        <Link className="navbar-brand" to="/">Avaleht</Link>
                                    </li>
                                    <li className="nav-item">
                                        <Link className="navbar-brand" to="/addevent">Ürituse lisamine</Link>
                                    </li>
                                </ul>
                                <ul className="navbar-nav">
                                    <img src={symbol.toString()} alt="React Logo" />
                                </ul>
                            </div>
                        </div>
                    </nav>
                </div>
                {/* {searchContext.redirect === true &&
                <Navigate to="/productsbyname"></Navigate>
            } */}
            </header>
        // </div>
    );
}

export default Header;