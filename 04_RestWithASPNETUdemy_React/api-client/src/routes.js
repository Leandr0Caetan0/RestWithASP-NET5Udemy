import React from "react";
import {BrowserRouter, Routes, Route} from 'react-router-dom';

import Login from './pages/Login';
import Books from './pages/Books';
import NewBook from './pages/NewBook';

export default function Rotas() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" Component={Login}/>
                <Route path="/books" Component={Books}/>
                <Route path="/book/new" Component={NewBook}/>
            </Routes>
        </BrowserRouter>
    );
}