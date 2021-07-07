import React,{Component} from 'react';
import {Modal,Button,Form} from 'react-bootstrap';
import {create,get, update} from '../helpers/apiHelpers';
import equal from 'fast-deep-equal';

export class NoteFormModal extends Component{
    constructor(props){
        super(props);
        this.handleSubmit=this.handleSubmit.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.state={
            categoriesToSend: [],
            categoriesFromDb: [],
            noteContent: '',
            categoriesId: [],
            noteid: '',
            note: {},
            updateNote: false,
        }
    }

    async componentDidMount(){
        await this.handleGetAllCategories();
    }

    async componentDidUpdate(prevProps, prevState){
        if(!equal(this.props.show, prevProps.show)){
            this.setState({categoriesId: []});
            this.setState({categoriesToSend: []});
            this.setState({updateNote: false});

            this.state.noteid = localStorage.getItem('noteId');
            
            if(this.state.noteid !== null){
                var note = await get('notes/' + this.state.noteid);
                this.setState({note: note});
                this.fillFormWhenNoteUpdate();
                this.setState({updateNote: true});
            }
        }
    }

    handleInputChange(event) {
        const target = event.target;
        var value = target.value;
        
        if(target.checked){
            this.setState({categoriesId: [...this.state.categoriesId, value]})
        }
        else{
            var index = this.state.categoriesId.indexOf(value);
            this.state.categoriesId.splice(index, 1)
        }
    }
    
    async handleSubmit(event){
        event.preventDefault();
        this.prepareDataToSend();
        await this.handleNoteRequest();
        this.props.onHide();
    }

    async prepareDataToSend(){
        var filteredCategoryIds = this.state.categoriesId.filter(function (el) {
            return el != null;
        });

        filteredCategoryIds.forEach(element => {
            this.state.categoriesToSend = [...this.state.categoriesToSend, {"id": element}];
        });
    }

    async handleNoteRequest(){
        var request = {
            "content": this.state.noteContent,
            "categories": this.state.categoriesToSend
        }
        if(this.state.updateNote){
            await update('notes/'+this.state.noteid, JSON.stringify(request));
        }
        else{
            await create('notes', JSON.stringify(request));
        }
    }

    async handleGetAllCategories(){
        let categories = await get('categories');
        this.setState({categoriesFromDb: categories});
    }

    fillFormWhenNoteUpdate(){
        var noteContent = document.getElementById('noteContent');

        if(noteContent != null && this.state.note.content)
        {
            this.setState({noteContent: this.state.note.content});
            noteContent.setAttribute('value', this.state.note.content);
        }

        this.state.note.categories?.forEach(element => {
            var checkbox = document.getElementById(element.id);
            if(checkbox != null)
            {
                checkbox.checked = true;
                this.setState({categoriesId: [...this.state.categoriesId, ''+element.id]});
            }
        });
    }

    render(){
        return(
            <div className="container">
                <Modal {...this.props} size="lg" aria-labelledby="contained-modal-title-vcenter" centered>
                <Modal.Header>
                            <Modal.Title id="contained-modal-title-vcenter">
                                {this.state.updateNote ? 'Update note' : 'Create note'}
                            </Modal.Title>
                        </Modal.Header>
                        <Modal.Body class="modal-body">
                                    <Form onSubmit={this.handleSubmit}>
                                        <Form.Group controlId="noteContent">
                                            <Form.Label>Content: </Form.Label>
                                            <Form.Control className="form-input" type="text" name="Content" required
                                            onChange = {(event) => this.setState({noteContent: event.target.value })}
                                            placeholder="Note content" />
                                        </Form.Group>

                                        <Form.Group controlId="categories">
                                            <Form.Label>Select categories: </Form.Label>
                                            {this.state.categoriesFromDb.map((category, index) => {
                                                return(
                                                    <Form.Check id={category.id} key={category.id} 
                                                        type="checkbox" label={category.name} value={category.id} onChange={this.handleInputChange}/>
                                                )
                                            })}
                                        </Form.Group>

                                        <Form.Group>
                                            <Button varian="primary" type="submit">
                                                {this.state.updateNote ? 'Update note' : 'Create note'}
                                            </Button>
                                        </Form.Group>
                                    </Form>
                        </Modal.Body>
                        <Modal.Footer>
                            <Button variant="danger" onClick={this.props.onHide}>Close</Button>
                        </Modal.Footer>
                </Modal>
            </div>
        )
    }
}