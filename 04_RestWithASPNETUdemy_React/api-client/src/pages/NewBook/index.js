import React from 'react';
import {Link} from 'react-router-dom';
import {Fi} from 'react-icons/fi';


import './styles.css';

import logoImage from '../../assets/logo.svg';

export default function NewBook(){
    return (
        <div className="new-book-container">
            <div className="content">
                <section className="form">
                    <img src={logoImage} alt='Erudio'/>
                    <h1>Add New Book</h1>
                    <p>Enter the book information and click on 'Add'!</p>
                    <Link className="back-link" to="/books">

                    </Link>
                </section>
            </div>
        </div>
    )
} 