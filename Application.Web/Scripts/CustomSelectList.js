class CustomSelectList {
    
    constructor(Args) {
        this._args = {};
        this._args = Args;
    }

    init() {
        debugger;
        $(this._args[0]).select2({
            ajax: {
                url: this._args[1],
                type: "POST",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    if (params.term == null) params.term = '';

                    return {
                        term: params.term,
                        q: params.term,// search term
                        page: params.page
                    };
                },
                results: function (data) {

                    return {
                        results: data

                    };
                },

                processResults: function (data, params) {

                    params.page = params.page || 1;
                    data = data.map(function (item) {
                        return {
                            id: item.id,
                            text: item.text,
                            otherField: item.otherField
                        }
                    });

                    return {
                        results: data,
                        pagination: {
                            more: (params.page * 30) < data.total_count
                        }
                    };
                },

                cache: true
            },
            placeholder: this._args[2],
            minimumInputLength: 0,
            width: '540',

        });
    }

    init2() {
        var Id = this._args[3];
        $(this._args[0]).select2({
            ajax: {
                url: this._args[1],
                type: "POST",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    if (params.term == null) params.term = '';
                    params.Id = Id;

                    return {
                        term: params.term,
                        id: params.Id,
                        q: params.term,// search term
                        page: params.page
                    };
                },
                results: function (data) {

                    return {
                        results: data

                    };
                },

                processResults: function (data, params) {

                    params.page = params.page || 1;
                    data = data.map(function (item) {
                        return {
                            id: item.id,
                            text: item.text,
                            otherField: item.otherField
                        }
                    });

                    return {
                        results: data,
                        pagination: {
                            more: (params.page * 30) < data.total_count
                        }
                    };
                },

                cache: true
            },
            placeholder: this._args[2],
            minimumInputLength: 0,
            width: '540',

        });
    }

    init3() {
        debugger;
        var code = this._args[3];
        $(this._args[0]).select2({
            ajax: {
                url: this._args[1],
                type: "POST",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    if (params.term == null) params.term = '';
                    params.cd = code;

                    return {
                        term: params.term,
                        code: params.cd,
                        q: params.term,// search term
                        page: params.page
                    };
                },
                results: function (data) {

                    return {
                        results: data

                    };
                },

                processResults: function (data, params) {

                    params.page = params.page || 1;
                    data = data.map(function (item) {
                        return {
                            id: item.id,
                            text: item.text,
                            otherField: item.otherField
                        }
                    });

                    return {
                        results: data,
                        pagination: {
                            more: (params.page * 30) < data.total_count
                        }
                    };
                },

                cache: true
            },
            placeholder: this._args[2],
            minimumInputLength: 0,
            width: '540',

        });
    }
}