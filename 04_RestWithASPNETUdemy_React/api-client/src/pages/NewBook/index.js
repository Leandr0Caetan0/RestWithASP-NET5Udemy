import React, {useEffect, useState} from 'react';
import {Link, useNavigate, useParams} from 'react-router-dom';
import {FiArrowLeft} from 'react-icons/fi';
import api from '../../services/api';


import './styles.css';

import logoImage from '../../assets/logo.svg';

export default function NewBook(){

    const[id, setId] = useState(null);
    const[title, setTitle] = useState('');
    const[author, setAuthor] = useState('');
    const[launchDate, setlaunchDate] = useState('');
    const[price, setPrice] = useState('');

    const navigate = useNavigate();

    const {bookId} = useParams();

    const accessToken = localStorage.getItem('accessToken');

    const authorization = {
        headers:{
            Authorization: "Bearer "+accessToken
        }
    };

    useEffect(() => {
        if (bookId === '0') {
            return;
        } else {
            loadBook();
        }
    }, bookId);

    async function loadBook() {
        try {
            const response = await api.get('api/book/v1/'+bookId, authorization)

            var adjustedDate = response.data.launchDate.split("T", 10)[0];    
            
            setId(response.data.id);
            setTitle(response.data.title);
            setAuthor(response.data.author);
            setlaunchDate(adjustedDate);
            setPrice(response.data.price);
        } catch (err) {
            alert('Error recovering book!')
            navigate('/books');
        }
    }

    async function saveOrUpdate(e) {
        e.preventDefault(); //Para pagina não atualizar no Submit. Manter SPA(Single Page Application)

        const data = {
            title,
            author,
            launchDate,
            price
        };

        try {
            if (bookId === '0') {
                await api.post('api/Book/v1', data, authorization);
            } else {
                data.id = id;
                await api.put('api/Book/v1', data, authorization);
            }
            
        } catch (error) {
            alert("Oh no! Wasn't possible create a new Book' :(");
        }

        navigate('/books');
    }
    return (
        <div className="new-book-container">
            <div className="content">
                <section className="form">
                    <img src={logoImage} alt='Erudio'/>
                    <h1>{bookId === '0'? 'Add New' : 'Update'} Book</h1>
                    <p>Enter the book information and click on '{bookId === '0'? 'Add' : 'Update'}'.</p>
                    <Link className="back-link" to="/books">
                        <FiArrowLeft size={16} color="#251FC5"/>
                        Back to Books
                    </Link>
                </section>

                <form onSubmit={saveOrUpdate}>
                    <input 
                        placeholder='Title'
                        value={title}
                        onChange={e => setTitle(e.target.value)}
                    />

                    <input 
                        placeholder='Author'
                        value={author}
                        onChange={e => setAuthor(e.target.value)}
                    />

                    <input 
                        type='date'
                        value={launchDate}
                        onChange={e => setlaunchDate(e.target.value)}
                    />

                    <input 
                        placeholder='Price' 
                        type='number'
                        value={price}
                        onChange={e => setPrice(e.target.value)}
                    />

                    <button className="button" type='submit'>{bookId === '0'? 'Add' : 'Update'}</button>
                </form>

            </div>
        </div>
    )
} 