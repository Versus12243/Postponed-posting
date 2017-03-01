class Common {
    static IsNullOrUndef(item) {
        return typeof item == 'undefined' || item == null;
    }

    static IsNullOrEmpty(item) {
        if (!Common.IsNullOrUndef(item)) {
            if (item == '')
                return true;
        }
        else
            return true;
        return false;
    }

    static hightLinght(item) {
        $(item).addClass('highlight');
        setTimeout(() => { $(item).removeClass('highlight'); }, 2000);
    }
}