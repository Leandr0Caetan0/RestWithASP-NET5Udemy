import React, {useState, useEffect} from "react";
import {Link, useNavigate} from 'react-router-dom';
import {FiPower, FiEdit, FiTrash2} from 'react-icons/fi';

import './styles.css';

import logoImage from '../../assets/logo.svg';

import api from '../../services/api';

export default function Books(){

    const[books, setBooks] = useState([]); //Muda a notação pois é um Array
    const[page, setPage] = useState(1); // Página começa em 1
    const navigate = useNavigate();

    const userName = localStorage.getItem('userName');
    const accessToken = localStorage.getItem('accessToken');

    const authorization = {
        headers:{
            Authorization: "Bearer "+accessToken
        }
    };

    useEffect(() => { // useEffect para exibir assim que a pagina carregar
        fetchMoreBooks();
    }, [accessToken]); // Monitora o accessToken

    async function fetchMoreBooks() {
        const response = await api.get('api/Book/v1/asc/4/'+page, authorization);
        setBooks([...books, ...response.data.list]); //concatena listas
        setPage(page + 1);
    }

    async function logout() {
        try {
            await api.get('api/auth/v1/revoke', authorization);

            localStorage.clear();

            navigate('/');
        } catch (err) {
            alert('Logout Failed!')
        }
    }

    async function editBook(id) {
        try {
            navigate('/book/new/'+id)
        } catch (err) {
            alert('Edit Failed!')
        }
    }

    async function deleteBook(id) {
        try {
            await api.delete('api/Book/v1/'+id, authorization);

            setBooks(books.filter(book => book.id !== id));
        } catch (err) {
            alert('Delete Failed!')
        }
    }

    return (
        <div className="book-container">
            <header>
                <img src={logoImage} alt='E Logo Book'/>
                <span>Welcome, <strong>{userName}</strong>!</span>
                <Link className="button" to="/book/new/0">Adicionar Livro</Link>
                <button type="button" onClick={logout}>
                    <FiPower size={18} color="#251FC5"/>
                </button>
            </header>

            <h1>Books List</h1>
            
            <ul>
                {books.map(book => (
                    <li key={book.id}>
                        <strong>Title:</strong>
                        <p>{book.title}</p>
                        <strong>Author:</strong>
                        <p>{book.author}</p>
                        <strong>Price:</strong>
                        <p>{Intl.NumberFormat('pt-BR', {style: 'currency', currency: 'BRL'}).format(book.price)}</p>
                        <strong>Release Date:</strong>
                        <p>{Intl.DateTimeFormat('pt-BR').format(new Date(book.launchDate))}</p>

                        <button type="button" onClick={() => editBook(book.id)}> {/* lambda no onClick para pegar apenas 1 book */}
                            <FiEdit size={20} color="#251FC5"/>
                        </button> 

                        <button type="button" onClick={() => deleteBook(book.id)}>
                            <FiTrash2 size={20} color="#251FC5"/>
                        </button>
                    </li>
                ))}
            </ul>
            <button className="button" onClick={fetchMoreBooks} type="button">Load More</button>
        </div>
    );
}