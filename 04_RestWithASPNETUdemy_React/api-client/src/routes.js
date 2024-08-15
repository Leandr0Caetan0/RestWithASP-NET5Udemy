import React from "react";
import {BrowserRouter, Routes, Route} from 'react-router-dom';

import Login from './pages/Login';
import Books from './pages/Books';

export default function Rotas() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" Component={Login}/>
                <Route path="/books" Component={Books}/>
            </Routes>
        </BrowserRouter>
    );
}